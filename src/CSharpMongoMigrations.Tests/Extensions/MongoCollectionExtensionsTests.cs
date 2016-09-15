using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using NUnit.Framework;
using System;
using System.Threading;

namespace CSharpMongoMigrations.Tests.Extensions
{
    [TestFixture(TestOf = typeof(MongoCollectionExtensions))]
    public class MongoCollectionExtensionsTests
    {
        [Test]
        public void UpdateTest()
        {
            // Arrange
            var document = new BsonDocument(new BsonElement("_id", Guid.NewGuid()));
            var mockCollection = new Mock<IMongoCollection<BsonDocument>>();

            mockCollection.Setup(x => x.ReplaceOne(It.IsAny<FilterDefinition<BsonDocument>>(), document, It.IsAny<UpdateOptions>(), It.IsAny<CancellationToken>()));
                       

            // Act
            mockCollection.Object.Update(document);

            // Assert
            mockCollection.Verify();
        }
    }
}
