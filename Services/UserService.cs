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
            if (user == null)
                throw  new Exception("Invalid user data.");
            
            string? userType = user.UserType;
            if (!userType.Equals(UserType.Client, StringComparison.CurrentCultureIgnoreCase) &&
                 !userType.Equals(UserType.Stylist, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("Invalid user type.");
            }

            try
            {
                UserRecord userRecord = await _firebaseAuth.CreateUserAsync(new UserRecordArgs
                {
                    Email = user.Email,
                    Password = user.Password,
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

                    transaction.Set(userRef, new
                    {
                        user.Email,
                        user.DisplayName,
                        user.PhoneNumber,
                        Password = new PasswordManager().HashPassword(user.Password),
                        UserType = user.UserType.ToString(),
                        Bio = string.Empty,
                        Styles = Array.Empty<string>()
                    });

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
                    .WhereEqualTo("Email", user.Email)
                    .GetSnapshotAsync();

                if (userSnapshot.Count == 0)
                {
                    throw new Exception("Invalid email or password.");
                }

                var snapshot = userSnapshot.Documents[0]; 
                UserLoginResponse userRecord = new()
                {
                    Id = snapshot.Id,
                    Email = snapshot.GetValue<string>("Email") ?? string.Empty,
                    DisplayName = snapshot.GetValue<string>("DisplayName") ?? string.Empty,
                    PhoneNumber = snapshot.GetValue<string>("PhoneNumber") ?? string.Empty,
                    UserType = snapshot.GetValue<string>("UserType") ?? string.Empty,
                    Bio = snapshot.GetValue<string>("Bio") ?? string.Empty,
                    Styles = snapshot.GetValue<List<string>>("Styles") ?? [],
                    CustomToken = string.Empty
                };

                string password = snapshot.GetValue<string>("Password") ?? string.Empty;
                if (!new PasswordManager().VerifyPassword(user.Password, password))
                {
                    throw new Exception("Invalid email or password.");
                }

                var customClaims = new Dictionary<string, object>
                {
                    { "UserId", userRecord.Id },
                };

                userRecord.CustomToken = new JwtTokenGenerator().GenerateToken(userRecord.Id);;
                return userRecord;
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
                if (user.DisplayName != null) userDict.Add("DisplayName", user.DisplayName);
                if (user.PhoneNumber != null) userDict.Add("PhoneNumber", user.PhoneNumber);
                if (user.Bio != null) userDict.Add("Bio", user.Bio);
                if (user.Styles != null) userDict.Add("Styles", user.Styles);

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
               
                return new UserResponse
                {
                    Id = userSnapshot.Id,
                    Email = userSnapshot.GetValue<string>("Email") ?? string.Empty,
                    DisplayName = userSnapshot.GetValue<string>("DisplayName") ?? string.Empty,
                    PhoneNumber = userSnapshot.GetValue<string>("PhoneNumber") ?? string.Empty,
                    UserType = userSnapshot.GetValue<string>("UserType") ?? string.Empty,
                    Bio = userSnapshot.GetValue<string>("Bio") ?? string.Empty,
                    Styles = userSnapshot.GetValue<List<string>>("Styles") ?? [],
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }  
}