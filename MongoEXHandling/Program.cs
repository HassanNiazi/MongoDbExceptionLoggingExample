using System.Web.Script.Serialization;
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;



namespace MongoEXHandling
{
    class Program
    {
        static void Main(string[] args)
        {
            IMongoClient _client;
            IMongoDatabase _database;

            _client = new MongoClient();
            _database = _client.GetDatabase("test");
      
            var ex = new Exception("Test Exception",new Exception("inner 1",new Exception("inner 2")));
            var collection = _database.GetCollection<BsonDocument>("exceptions");
            var bsonEx = ex.ToBsonDocument();
            var json = new JavaScriptSerializer().Serialize(ex);
            Console.WriteLine();
            Console.WriteLine(json);
            Console.WriteLine();
            var bd = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(json);
            Console.WriteLine(bd);
            collection.InsertOneAsync(bd).Wait();
            Console.ReadLine();
            Read(collection);
            Console.ReadLine();
        }

        public static void Read(IMongoCollection<BsonDocument> collection)
        {
            var filter = new BsonDocument();
            var cursor = collection.Find(filter);

            foreach (var item in cursor.ToList())
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
