using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace CSharpMongoMigrations
{
    /// <summary>
    /// The base class for a document migration
    /// Migrate each document in target collection
    /// </summary>
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

        /// <summary>
        /// Upgrade the specified document
        /// </summary>
        /// <param name="document"></param>
        protected abstract void UpgradeDocument(BsonDocument document);

        /// <summary>
        /// Downgrade the specified document
        /// </summary>
        /// <param name="document"></param>
        protected abstract void DowngradeDocument(BsonDocument document);
    }
}