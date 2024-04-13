using Google.Cloud.Firestore;
using online_fashion_shopping_api.Models;
using online_fashion_shopping_api.Responses;

namespace online_fashion_shopping_api.Services
{
    public class ChatService(FirestoreDb firestoreDb)
    {
         private readonly FirestoreDb _firestoreDb = firestoreDb;

        public async Task<Conversation> InitiateChat(ChatInitiateRequest request)
         {
             DocumentReference _conversationRef = _firestoreDb.Collection("chats").Document();
             Dictionary<string, object> conversation = new()
             {
                { "members", new string[] { request.SenderId, request.ReceiverId } },
             };
             await _conversationRef.SetAsync(conversation);

              CollectionReference _messagesRef = _conversationRef.Collection("messages");
              DocumentReference _messageRef = _messagesRef.Document();
              Dictionary<string, object> message = new()
              {
                  { "sender_id", request.SenderId },
                  { "content", "Hello!" },
                  { "timestamp", FieldValue.ServerTimestamp }
              };

              await _messageRef.SetAsync(message);

              return new Conversation
              {
                  Id = _conversationRef.Id,
                  Members = [request.SenderId, request.ReceiverId],
                  Messages =
                  [
                      new() {
                          Id = _messageRef.Id,
                          SenderId = request.SenderId,
                          Content = "Hello!",
                          Timestamp = DateTime.Now
                      }
                  ]
              };
           
         }

        public async Task<Conversation?> SendMessage(SendMessageRequest messageRequest)
         {
             DocumentReference _conversationRef = _firestoreDb.Collection("chats").Document(messageRequest.ConversationId);
             CollectionReference _messagesRef = _conversationRef.Collection("messages");
             DocumentReference _messageRef = _messagesRef.Document();
             Dictionary<string, object> message = new()
             {
                 { "sender_id", messageRequest.SenderId },
                 { "content", messageRequest.Content },
                 { "timestamp", FieldValue.ServerTimestamp }
             };

             await _messageRef.SetAsync(message);

            return await GetConversation(messageRequest.ConversationId);
         }

            public async Task<Conversation?> GetConversation(string conversationId)
            {
                DocumentReference _ref = _firestoreDb.Collection("chats").Document(conversationId);
                DocumentSnapshot _doc = await _ref.GetSnapshotAsync();
    
                if(!_doc.Exists) return null;

                QuerySnapshot _messagesSnapshot = await _ref.Collection("messages").GetSnapshotAsync();
                Message[] _messages = _messagesSnapshot.Documents.Select(Message.FromFirestore).ToArray();

                return new Conversation
                {
                    Id = _doc.Id,
                    Members = _doc.GetValue<string[]>("members"),
                    Messages = _messages
                };
            }
    }
}