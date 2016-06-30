using MongoDB.Bson;
using System;

namespace CSharpMongoMigrations.Extensions
{
    public static class BsonDocumentExtensions
    {
        public static void RenameProperty(this BsonDocument document, string oldName, string newName)
        {
            var old = document.GetElement(oldName);
            var @new = new BsonElement(newName, BsonValue.Create(old.Value));

            document.Remove(oldName);
            document.Add(@new);
        }

        public static void RemoveProperty(this BsonDocument document, string name)
        {
            document.Remove(name);
        }

        public static void AddProperty(this BsonDocument document, string name, object value)
        {
            var element = new BsonElement(name, BsonValue.Create(value));
            document.Add(element);
        }

        public static void AddUniqueIdentifier(this BsonDocument document, Guid value)
        {
            document.AddProperty("_id", value);
        }
                
        public static BsonValue GetValue(this BsonDocument document, string name)
        {
            return document.GetElement(name).Value;
        }
        
        public static void ChangeValue(this BsonDocument document, string property, object value)
        {
            var @new = new BsonElement(property, BsonValue.Create(value));

            document.Remove(property);
            document.Add(@new);
        }        
    }
}
