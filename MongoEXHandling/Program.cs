﻿using MongoDB.Bson;
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
            Console.Read();
        }

        public static void Read(IMongoCollection<BsonDocument> collection)
        {
            foreach (var document in collection.Find(new BsonDocument()).ToList())
            {
                foreach (var element in document)
                {
                    Console.WriteLine(element);
                }
                Console.WriteLine("-------------------------------------------");
            }
        }
    }
}
