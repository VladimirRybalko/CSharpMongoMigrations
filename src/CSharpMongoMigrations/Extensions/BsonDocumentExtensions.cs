using MongoDB.Bson;
using System;

namespace CSharpMongoMigrations
{
    public static class BsonDocumentExtensions
    {
        /// <summary>
        /// Rename document property
        /// </summary>
        /// <param name="document"></param>
        /// <param name="oldName">The old property name</param>
        /// <param name="newName">The new property name</param>
        public static void RenameProperty(this BsonDocument document, string oldName, string newName)
        {
            var old = document.GetElement(oldName);
            var @new = new BsonElement(newName, BsonValue.Create(old.Value));

            document.Remove(oldName);
            document.Add(@new);
        }

        /// <summary>
        /// Remove property from document
        /// </summary>
        /// <param name="document"></param>
        /// <param name="property">The property name</param>
        public static void RemoveProperty(this BsonDocument document, string property)
        {
            document.Remove(property);
        }

        /// <summary>
        /// Add new property to document
        /// </summary>
        /// <param name="document"></param>
        /// <param name="property">The property name</param>
        /// <param name="value">The property value</param>
        public static void AddProperty(this BsonDocument document, string property, object value)
        {
            var element = new BsonElement(property, BsonValue.Create(value));
            document.Add(element);
        }


        /// <summary>
        /// Add the unique identifier for document
        /// </summary>
        /// <param name="document"></param>
        /// <param name="value">Unique identifier value</param>
        /// <remarks>Id is always Guid value by convention</remarks>
        public static void AddUniqueIdentifier(this BsonDocument document, Guid value)
        {
            document.AddProperty("_id", value);
        }

        /// <summary>
        /// Get document property value
        /// </summary>
        /// <param name="document"></param>
        /// <param name="property">The property name</param>
        /// <returns></returns>
        public static BsonValue GetValue(this BsonDocument document, string property)
        {
            return document.GetElement(property).Value;
        }
        
        /// <summary>
        /// Change property value
        /// </summary>
        /// <param name="document"></param>
        /// <param name="property">The property name</param>
        /// <param name="value"></param>
        public static void ChangeValue(this BsonDocument document, string property, object value)
        {
            var @new = new BsonElement(property, BsonValue.Create(value));

            document.Remove(property);
            document.Add(@new);
        }        
    }
}
