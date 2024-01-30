using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Net.Mail;
using System.Net;
namespace API
{
    public class MyMethods
    {
        public static string generateCode()
        {
            Random random = new Random();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 6; i++)
            {
                builder.Append(random.Next(0, 9).ToString());
            }
            return builder.ToString();
        }
        public static bool checkExistAndValid(string email, string code)
        {
            var client = new MongoClient("mongodb+srv://fy:NadimirVlagr69@cluster0.i0urxgw.mongodb.net/?retryWrites=true&w=majority");
            var database = client.GetDatabase("checks");
            var collection = database.GetCollection<BsonDocument>("emails");
            var projection = Builders<BsonDocument>.Projection.Include("date").Include("code").Exclude("_id");
            var filter = Builders<BsonDocument>.Filter.Eq("email", email);
            var result = collection.Find(filter).Project(projection).FirstOrDefault();
            if (result != null)
            {
                if(result.TryGetValue("date", out BsonValue date))
                {
                    if(DateTime.Now < date)
                    {
                        if(result.TryGetValue("code", out BsonValue codeAnother))
                        {
                            if(codeAnother.Equals(code))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }
        public static void createRecord(string email, string code, DateTime date)
        {
            var client = new MongoClient("mongodb+srv://fy:NadimirVlagr69@cluster0.i0urxgw.mongodb.net/?retryWrites=true&w=majority");
            var database = client.GetDatabase("checks");
          var collection = database.GetCollection<BsonDocument>("emails");
          collection.InsertOne(new BsonDocument { { "email", email }, { "code", code }, { "date", date } });
        }
        public static void updateVertificationEmail(string email) {
            var client = new MongoClient("mongodb+srv://fy:NadimirVlagr69@cluster0.i0urxgw.mongodb.net/?retryWrites=true&w=majority");
            var database = client.GetDatabase("test");
            var collection = database.GetCollection<BsonDocument>("users");
            var filter = Builders<BsonDocument>.Filter.Eq("email", email);
            var update = Builders<BsonDocument>.Update.Set("vertification", true);
            collection.UpdateOne(filter, update);
        }
        public static void sendCode(string email, string code)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                UseDefaultCredentials = false,

                Credentials = new NetworkCredential("wapit.cz@gmail.com", "pyisndvleqssvfjc"),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,

            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("wapit.cz@gmail.com"),
                Subject = "Ověřovací kód",
                Body = "Váš ověřovací kód je: <h3>" + code + "</h3>",
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);
            smtpClient.Send(mailMessage);
        }
        public static bool existEmail(string email)
        {
            var client = new MongoClient("mongodb+srv://fy:NadimirVlagr69@cluster0.i0urxgw.mongodb.net/?retryWrites=true&w=majority");
            var database = client.GetDatabase("checks");
            var collection = database.GetCollection<BsonDocument>("emails");
            var filter = Builders<BsonDocument>.Filter.Eq("email", email);
            var result = collection.Find(filter).FirstOrDefault();
            if (result != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
            
    }
}
