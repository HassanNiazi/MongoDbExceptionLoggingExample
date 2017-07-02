using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Web.Script.Serialization;

namespace MongoEXHandling
{
    class Program
    {
        static void Main(string[] args)
        {
            var collection = new MongoClient().GetDatabase("test").GetCollection<BsonDocument>("exceptions");
            var ex = new Exception("Test Exception", new Exception("inner 1", new Exception("inner 2")));
            var json = new JavaScriptSerializer().Serialize(ex);
            var bson = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(json);
            collection.InsertOneAsync(bson).Wait();
            Read(collection);
        }

        public static void Read(IMongoCollection<BsonDocument> collection)
        {
            foreach (var item in collection.Find(new BsonDocument()).ToList())
            {
                foreach (var document in item)
                {
                    Console.WriteLine(document);
                    Console.WriteLine();
                }
            }
        }
    }
}
