using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using System;
using System.Threading;
using Xunit;

namespace CSharpMongoMigrations.Tests
{   
    public class MongoCollectionExtensionsFacts
    {
        [Fact]
        public void UpdateFact()
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
