using MongoDB.Bson;
using System;

namespace CSharpMongoMigrations
{
    /// <summary>
    /// Defines bson document custom extensions
    /// </summary>
    public static class BsonDocumentExtensions
    {
        /// <summary>
        /// Rename a document property.
        /// </summary>
        /// <param name="document">The target document.</param>
        /// <param name="oldName">The old property name.</param>
        /// <param name="newName">The new property name.</param>
        public static void RenameProperty(this BsonDocument document, string oldName, string newName)
        {
            var old = document.GetElement(oldName);
            var @new = new BsonElement(newName, BsonValue.Create(old.Value));

            document.Remove(oldName);
            document.Add(@new);
        }

        /// <summary>
        /// Remove a property from the document.
        /// </summary>
        /// <param name="document">The target document.</param>
        /// <param name="property">The desired property name.</param>
        public static void RemoveProperty(this BsonDocument document, string property)
        {
            document.Remove(property);
        }

        /// <summary>
        /// Add new property to the document.
        /// </summary>
        /// <param name="document">The target document.</param>
        /// <param name="property">The desired property name.</param>
        /// <param name="value">The desired property value.</param>
        public static void AddProperty(this BsonDocument document, string property, object value)
        {
            var element = new BsonElement(property, BsonValue.Create(value));
            document.Add(element);
        }


        /// <summary>
        /// Add the unique identifier to the document.
        /// </summary>
        /// <param name="document">The target document.</param>
        /// <param name="value">An unique identifier value.</param>
        /// <remarks>Id is always a Guid value by convention.</remarks>
        public static void AddUniqueIdentifier(this BsonDocument document, Guid value)
        {
            document.AddProperty("_id", value);
        }

        /// <summary>
        /// Change the property value.
        /// </summary>
        /// <param name="document">The target document.</param>
        /// <param name="property">The desired property name.</param>
        /// <param name="value"></param>
        public static void ChangeValue(this BsonDocument document, string property, object value)
        {
            var @new = new BsonElement(property, BsonValue.Create(value));

            document.Remove(property);
            document.Add(@new);
        }        
    }
}