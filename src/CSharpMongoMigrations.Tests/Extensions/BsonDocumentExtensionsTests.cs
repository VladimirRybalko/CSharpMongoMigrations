using FluentAssertions;
using MongoDB.Bson;
using System;
using Xunit;

namespace CSharpMongoMigrations.Tests
{
    public class BsonDocumentExtensionsFacts 
    {
        [Fact]
        public void RenamePropertyFact()
        {
            // Arrange
            var oldName = AutoFixture.String();
            var newName = AutoFixture.String();
            var value = AutoFixture.String();
            var document = new BsonDocument(new BsonElement(oldName, value));

            // Act
            document.RenameProperty(oldName, newName);
            var @new = document.GetElement(newName);

            // Assert
            Action act = () => document.GetElement(oldName);            
            act.Should().Throw<Exception>();
            @new.Should().NotBeNull();
            @new.Value.Should().Be(value);
        }

        [Fact]
        public void RemovePropertyFact()
        {
            // Arrange
            var name = AutoFixture.String();
            var document = new BsonDocument(new BsonElement(name, string.Empty));

            // Act
            document.RemoveProperty(name);
            // Assert
            Action act = () => document.GetElement(name);
            act.Should().Throw<Exception>();
        }

        [Fact]
        public void AddPropertyFact()
        {
            // Arrange
            var name = AutoFixture.String();
            var value = AutoFixture.Int();
            var document = new BsonDocument();

            // Act
            document.AddProperty(name, value);
            var element = document.GetElement(name);

            // Assert
            element.Should().NotBeNull();
            element.Value.Should().Be(value);
        }

        [Fact]
        public void AddPropertyGuidFact()
        {
            // Arrange
            var name = AutoFixture.String();
            var value = Guid.NewGuid();
            var document = new BsonDocument();

            // Act
            document.AddProperty(name, value);
            var element = document.GetElement(name);

            // Assert
            element.Should().NotBeNull();
            element.Value.AsGuid.Should().Be(value);
        }

        [Fact]
        public void AddPropertyGuidWithStandardGuidRepresentationFact()
        {
            // Arrange
            var name = AutoFixture.String();
            var value = Guid.NewGuid();
            var document = new BsonDocument();

            // Act
            document.AddProperty(name, value, GuidRepresentation.Standard);
            var element = document.GetElement(name);

            // Assert
            element.Should().NotBeNull();
            element.Value.AsGuid.Should().Be(value);
        }

        [Fact]
        public void AddUniqueIdentifierFact()
        {
            // Arrange
            var value = Guid.NewGuid();
            var document = new BsonDocument();

            // Act
            document.AddUniqueIdentifier(value);
            var element = document.GetElement("_id");

            // Assert
            element.Should().NotBeNull();
            element.Value.AsGuid.Should().Be(value);
        }

        [Fact]
        public void AddUniqueIdentifierWithStandardGuidRepresentationFact()
        {
            // Arrange
            var value = Guid.NewGuid();
            var document = new BsonDocument();
            var guidRepresentation = GuidRepresentation.Standard;

            // Act
            document.AddUniqueIdentifier(value, guidRepresentation);
            var element = document.GetElement("_id");

            // Assert
            element.Should().NotBeNull();
            element.Value.AsGuid.Should().Be(value);
        }

        [Fact]
        public void ChangeValueFact()
        {
            // Arrange
            var name = AutoFixture.String();
            var oldValue = AutoFixture.Int();
            var newValue = AutoFixture.Int();
            var document = new BsonDocument(new BsonElement("_id", new BsonBinaryData(Guid.NewGuid(), GuidRepresentation.Standard)));

            // Act
            document.AddProperty(name, oldValue);
            document.ChangeValue(name, newValue);
            var result = document.GetValue(name);

            // Assert
            result.AsInt32.Should().Be(newValue);
        }

        [Fact]
        public void ChangeValueGuidFact()
        {
            // Arrange
            var name = AutoFixture.String();
            var oldValue = Guid.NewGuid();
            var newValue = Guid.NewGuid();
            var document = new BsonDocument(new BsonElement("_id", new BsonBinaryData(Guid.NewGuid(), GuidRepresentation.Standard)));

            // Act
            document.AddProperty(name, oldValue);
            document.ChangeValue(name, newValue);
            var result = document.GetValue(name);

            // Assert
            result.AsGuid.Should().Be(newValue);
        }

        [Fact]
        public void ChangeValueGuidWithStandardGuidRepresentationFact()
        {
            // Arrange
            var name = AutoFixture.String();
            var oldValue = Guid.NewGuid();
            var newValue = Guid.NewGuid();
            var document = new BsonDocument(new BsonElement("_id", new BsonBinaryData(Guid.NewGuid(), GuidRepresentation.Standard)));

            // Act
            document.AddProperty(name, oldValue);
            document.ChangeValue(name, newValue, GuidRepresentation.Standard);
            var result = document.GetValue(name);

            // Assert
            result.AsGuid.Should().Be(newValue);
        }
    }
}
