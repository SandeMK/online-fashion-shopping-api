using Google.Cloud.Firestore;
using online_fashion_shopping_api.Models;

namespace online_fashion_shopping_api.Services
{
    public class StyleService(FirestoreDb firestoreDb)
    {
         private readonly FirestoreDb _firestoreDb = firestoreDb;

        public async Task<List<Style>> GetStyles()
         {
             CollectionReference colelctionRef = _firestoreDb.Collection("styles");
             QuerySnapshot styles = await colelctionRef.GetSnapshotAsync();

             List<Style> stylesList = styles.Documents.Select((styleDoc) => {
                return new Style()
                {
                    Id = styleDoc.Id,
                    Name = styleDoc.GetValue<string>("name"),
                    Description = styleDoc.GetValue<string>("description"),
                    ImageUrl = styleDoc.GetValue<string>("image_url")
                };
             }).ToList();

             return stylesList;
         }

            public async Task<Style?> GetStyleById(string id)
            {
                DocumentReference styleRef = _firestoreDb.Collection("styles").Document(id);
                DocumentSnapshot styleDoc = await styleRef.GetSnapshotAsync();
    
                if (styleDoc.Exists)
                {
                    return new Style()
                    {
                        Id = styleDoc.Id,
                        Name = styleDoc.GetValue<string>("name"),
                        Description = styleDoc.GetValue<string>("description"),
                        ImageUrl = styleDoc.GetValue<string>("image_url")
                    };
                }
                return null;
            }
    }
}