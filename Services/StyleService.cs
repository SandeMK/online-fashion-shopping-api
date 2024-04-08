using Google.Cloud.Firestore;
using online_fashion_shopping_api.Models;

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

            public async Task<Style?> GetStyleById(string id)
            {
                DocumentReference _ref = _firestoreDb.Collection("styles").Document(id);
                DocumentSnapshot _doc = await _ref.GetSnapshotAsync();
    
                if (_doc.Exists)
                {
                    return Style.FromFirestore(_doc);
                }
                return null;
            }
    }
}