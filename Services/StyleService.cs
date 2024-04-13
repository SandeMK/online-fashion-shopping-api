using Google.Cloud.Firestore;
using online_fashion_shopping_api.Models;
using online_fashion_shopping_api.Responses;

namespace online_fashion_shopping_api.Services
{
    public class StyleService(FirestoreDb firestoreDb)
    {
         private readonly FirestoreDb _firestoreDb = firestoreDb;

        public async Task<List<Style>> GetStyles()
         {
             CollectionReference _ref = _firestoreDb.Collection("styles");
             QuerySnapshot styles = await _ref.GetSnapshotAsync();
             
             return styles.Documents.Select(Style.FromFirestore).ToList();
         }

            public async Task<GetStyleResponse?> GetStyleById(string id)
            {
                DocumentReference _ref = _firestoreDb.Collection("styles").Document(id);
                DocumentSnapshot _doc = await _ref.GetSnapshotAsync();
    
                // get all stylists associated with the style
                QuerySnapshot stylists = await _firestoreDb.Collection("users")
                    .WhereArrayContains("styles", id)
                    .GetSnapshotAsync();

                UserResponse[] _stylists = stylists.Documents.Select(UserResponse.FromFirestore).ToArray();

                return _doc.Exists ? new GetStyleResponse
                {
                    Style = Style.FromFirestore(_doc),
                    Stylists = _stylists
                } : null;
            }
    }
}