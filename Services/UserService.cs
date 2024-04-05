using FirebaseAdmin.Auth;
using Google.Cloud.Firestore;
using online_fashion_shopping_api.Models;
using online_fashion_shopping_api.Requests;
using online_fashion_shopping_api.Utilities;

namespace online_fashion_shopping_api.Services
{
    public class UserService(FirebaseAuth firebaseAuth, FirestoreDb firestoreDb)
    {
        private readonly FirebaseAuth _firebaseAuth = firebaseAuth;
        private readonly FirestoreDb _firestoreDb = firestoreDb;

        public async Task<object> Register(UserRegistrationRequest user)
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
                        UserType = user.UserType.ToString()
                    });

                    return transaction;
                });

                return new
                {
                    Id=userRecord.Uid,
                    user.Email,
                    user.DisplayName,
                    user.PhoneNumber,
                    user.UserType,
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<object> Login(UserLoginRequest user)
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

                Dictionary<string, object> userDict = userSnapshot.Documents[0].ToDictionary();
                User userRecord = new()
                {
                    Id = userSnapshot.Documents[0].Id,
                    Email = userDict["Email"].ToString() ?? string.Empty,
                    Password = userDict["Password"].ToString() ?? string.Empty,
                    DisplayName = userDict["DisplayName"].ToString() ?? string.Empty,
                    PhoneNumber = userDict["PhoneNumber"].ToString() ?? string.Empty,
                    UserType = userDict["UserType"].ToString() ?? string.Empty,
                };

                if (!new PasswordManager().VerifyPassword(user.Password, userRecord.Password))
                {
                    throw new Exception("Invalid email or password.");
                }

                var customClaims = new Dictionary<string, object>
                {
                    { "UserId", userRecord.Id },
                };

                string customToken = new JwtTokenGenerator().GenerateToken(userRecord.Id);
                return new
                {
                    userRecord.Id,
                    userRecord.Email,
                    userRecord.DisplayName,
                    userRecord.PhoneNumber,
                    userRecord.UserType,
                    customToken
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }  
}