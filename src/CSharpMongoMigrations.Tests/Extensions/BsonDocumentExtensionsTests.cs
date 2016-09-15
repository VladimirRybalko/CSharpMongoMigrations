using FluentAssertions;
using MongoDB.Bson;
using NUnit.Framework;
using System;

namespace CSharpMongoMigrations.Tests
{
    [TestFixture(TestOf = typeof(BsonDocumentExtensions))]
    public class BsonDocumentExtensionsTests 
    {
        [Test]
        public void RenamePropertyTest()
        {
            // Arrange
            var oldName = AutoFixture.Create<string>();
            var newName = AutoFixture.Create<string>();
            var value = AutoFixture.Create<int>();
            var document = new BsonDocument(new BsonElement(oldName, value));

            // Act
            document.RenameProperty(oldName, newName);
            var @new = document.GetElement(newName);

            // Assert
            Action act = () => document.GetElement(oldName);
            act.ShouldThrow<Exception>();
            @new.Should().NotBeNull();
            @new.Value.Should().Be(value);
        }

        [Test]
        public void RemovePropertyTest()
        {
            // Arrange
            var name = AutoFixture.Create<string>();
            var document = new BsonDocument(new BsonElement(name, string.Empty));

            // Act
            document.RemoveProperty(name);
            // Assert
            Action act = () => document.GetElement(name);
            act.ShouldThrow<Exception>();
        }


        [Test]
        public void AddPropertyTest()
        {
            // Arrange
            var name = AutoFixture.Create<string>();
            var value = AutoFixture.Create<int>();
            var document = new BsonDocument();

            // Act
            document.AddProperty(name, value);
            var element = document.GetElement(name);

            // Assert
            element.Should().NotBeNull();
            element.Value.Should().Be(value);
        }

        [Test]
        public void AddUniqueIdentifierTest()
        {
            // Arrange
            var value = Guid.NewGuid();
            var document = new BsonDocument();

            // Act
            document.AddUniqueIdentifier(value);
            var element = document.GetElement("_id");

            // Assert
            element.Should().NotBeNull();
            element.Value.Should().Be(value);
        }

        [Test]
        public void ChangeValueTest()
        {
            // Arrange
            var name = AutoFixture.Create<string>();
            var oldValue = Guid.NewGuid();
            var newValue = Guid.NewGuid();
            var document = new BsonDocument(new BsonElement(name, oldValue));

            // Act
            document.ChangeValue(name, newValue);
            var result = document.GetValue(name);

            // Assert
            result.Should().Be(newValue);
        }
    }
}
