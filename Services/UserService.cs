using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;
using online_fashion_shopping_api.Models;
using online_fashion_shopping_api.Requests;
using online_fashion_shopping_api.Responses;
using online_fashion_shopping_api.Utilities;

namespace online_fashion_shopping_api.Services
{
    public class UserService(FirebaseAuth firebaseAuth, FirestoreDb firestoreDb)
    {
        private readonly FirebaseAuth _firebaseAuth = firebaseAuth;
        private readonly FirestoreDb _firestoreDb = firestoreDb;

        public async Task<UserResponse> Register(UserRegistrationRequest user)
        {
            if (user == null) {
                throw  new Exception("Invalid user data.");
            }
            
            string? userType = user.UserType;
            if (!userType.Equals(UserType.Client, StringComparison.CurrentCultureIgnoreCase) &&
                 !userType.Equals(UserType.Stylist, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("Invalid user type.");
            }

            try
            {
                string hashedPassword = new PasswordManager().HashPassword(user.Password);
                UserRecord userRecord = await _firebaseAuth.CreateUserAsync(new UserRecordArgs
                {
                    Email = user.Email,
                    Password = hashedPassword,
                    DisplayName = user.DisplayName
                });

                await _firestoreDb.RunTransactionAsync(async transaction =>
                {
                    DocumentReference userRef = _firestoreDb.Collection("users").Document(userRecord.Uid);
                    DocumentSnapshot userSnapshot = await transaction.GetSnapshotAsync(userRef);

                    if (userSnapshot.Exists)
                    {
                        throw new Exception("User already exists.");
                    }

                    CreateUser _user = new()
                    {
                        Email = user.Email,
                        DisplayName = user.DisplayName,
                        PhoneNumber = user.PhoneNumber,
                        UserType = user.UserType,
                        Password = hashedPassword
                    };

                    transaction.Set(userRef, _user.ToDictionary());

                    return transaction;
                });

                return new UserResponse
                {
                    Id = userRecord.Uid,
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    PhoneNumber = user.PhoneNumber,
                    UserType = user.UserType,
                    Bio = string.Empty,
                    Styles = [],
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserLoginResponse> Login(UserLoginRequest user)
        {
            if (user == null) throw new Exception("Invalid user data.");
            try
            {
                QuerySnapshot userSnapshot = await _firestoreDb.Collection("users")
                    .WhereEqualTo("email", user.Email)
                    .GetSnapshotAsync();

                if (userSnapshot.Count == 0)
                {
                    throw new Exception("Invalid email or password.");
                }

                DocumentSnapshot snapshot = userSnapshot.Documents[0]; 
                if (!new PasswordManager().VerifyPassword(user.Password, snapshot.GetValue<string>("password")))
                {
                    throw new Exception("Invalid email or password.");
                }

                var customClaims = new Dictionary<string, object>
                {
                    { "user_id", snapshot.Id },
                };
                string token = new JwtTokenGenerator().GenerateToken(snapshot.Id);
                return UserLoginResponse.FromFirestore(snapshot, token);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserResponse> UpdateProfile(string userId, UserUpdateRequest user)
        {
            if (user == null)
                throw new Exception("Invalid user data.");
            try
            {
                DocumentReference userRef = _firestoreDb.Collection("users").Document(userId);
                DocumentSnapshot userSnapshot = await userRef.GetSnapshotAsync();

                if (!userSnapshot.Exists)
                {
                    throw new Exception("User not found.");
                }
              
                Dictionary<string, object> userDict = [];
                if (user.DisplayName != null) userDict.Add("display_name", user.DisplayName);
                if (user.PhoneNumber != null) userDict.Add("phone_number", user.PhoneNumber);
                if (user.Bio != null) userDict.Add("bio", user.Bio);
                if (user.Styles != null) userDict.Add("styles", user.Styles);

                if(userDict.Count == 0) {
                    throw new Exception("No data to update.");
                }

                await userRef.UpdateAsync(userDict);
                return await GetProfile(userId);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<UserResponse> GetProfile(string userId)
        {
            try
            {
                DocumentReference userRef = _firestoreDb.Collection("users").Document(userId);
                DocumentSnapshot userSnapshot = await userRef.GetSnapshotAsync();

                if (!userSnapshot.Exists)
                {
                    throw new Exception("User not found.");
                }
               
                return UserResponse.FromFirestore(userSnapshot);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }  
}