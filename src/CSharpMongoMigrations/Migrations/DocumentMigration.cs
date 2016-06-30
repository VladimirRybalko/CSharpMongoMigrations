using CSharpMongoMigrations.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace CSharpMongoMigrations
{
    public abstract class DocumentMigration : Migration 
    {
        protected abstract string CollectionName { get; }
        
        public override void Up()
        {
            Update(UpgradeDocument);
        }

        public override void Down()
        {
            Update(DowngradeDocument);
        }

        private void Update(Action<BsonDocument> action)
        {
            var collection = GetCollection();
            var documents = GetDocuments();

            foreach (var document in documents)
            {
                action(document);
                collection.Update(document);
            }
        }
        protected IMongoCollection<BsonDocument> GetCollection()
        {
            return GetCollection(CollectionName);
        }

        protected virtual List<BsonDocument> GetDocuments()
        {
            return GetCollection().FindAll().ToList();
        }

        protected virtual void UpgradeDocument(BsonDocument document) { }
        protected virtual void DowngradeDocument(BsonDocument document) { }
    }
}
