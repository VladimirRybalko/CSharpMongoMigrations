# CSharpMongoMigrations

[![NuGet](https://img.shields.io/badge/nuget-2.5.2-blue.svg)](https://www.nuget.org/packages/CSharpMongoMigrations/)

## What is it?

**CSharpMongoMigrations** is an alternative .NET library for MongoDB migrations. Unlike the official package, it supports both Up and Down migrations, which can be a significant advantage for large-scale MongoDB projects. In addition, it has no dependencies on outdated MongoDB drivers, so there's no need to manage multiple driver versions anymore.


## Documentation API
Generally, there are four main types of migrations:

1) **Common migration**

```csharp
   [Migration(0, "Add John Doe")]
    public class AddPersonMigration : Migration
    {
        public override void Up()
        {
            var collection = GetCollection("Persons");
            var document = new BsonDocument();

            document.AddUniqueIdentifier(new Guid("06BFFCF5-DAE9-422A-85AB-F58DE41E86DA"));
            document.AddProperty("Name", "John Doe");
            
            collection.InsertOne(document);
        }

        public override void Down()
        {
            var collection = GetCollection("Persons");
            var idFilter = Builders<BsonDocument>.Filter.Eq("_id", new Guid("06BFFCF5-DAE9-422A-85AB-F58DE41E86DA"));
            collection.DeleteOne(idFilter);
        }
    }
```

In this example, we define the **Up** migration to add John Doe to the Person collection. The **Down** method rolls back the migration, restoring the database to its original state.
Take note of the **Migration** attribute — it's required for the migration to be executed by the launcher. You must specify a unique migration number (e.g., <span style="color:gray">'0'</span>) and provide a descriptive label (e.g., <span style="color:gray">'Add John Doe'</span>).


2) **Document migration**

These migrations allow to apply changes to each document in the specified collection.
```csharp
   [Migration(1, "Change persons")]
    public class AddPropertyPersonMigration : DocumentMigration
    {
        protected override string CollectionName { get { return "Persons"; } }
               
        protected override void UpgradeDocument(BsonDocument document)
        {
            document.AddProperty("IsActive", true);            
        }

        protected override void DowngradeDocument(BsonDocument document)
        {
            document.RemoveProperty("IsActive");
        }
    }
```


3) **Collection migration**

These migrations allow to apply changes to each collection separately from another ones.
```csharp
   [Migration("Animals", 0)]
    public sealed class AddAnimalMigration : Migration
    {
        public override void Up()
        {
            var collection = GetCollection("Animals");
            var document = new BsonDocument();

            document.AddUniqueIdentifier(new Guid("2A7B73A8-3C4A-422D-90B4-C73BCF48EBD4"));
            document.AddProperty("Kind", "Cat");

            collection.InsertOne(document);
        }

        public override void Down()
        {
            var collection = GetCollection("Animals");
            var idFilter = Builders<BsonDocument>.Filter.Eq("_id", new Guid("2A7B73A8-3C4A-422D-90B4-C73BCF48EBD4"));
            collection.DeleteOne(idFilter);
        }
    }
```
Here, the *Migration* attribute specifies the target collection name and the migration version for a predefined collection. This enables migrations to be applied to a specific schema.

4) **Conditional migration**

These migrations can be conditionally skipped based on predefined criteria.
```csharp
    [Migration(4, "Add Migration when condition meets")]
    public sealed class ConditionalMigrations : Migration
    {
        public override void Up()
        {
            var collection = GetCollection("Persons");
            var document = new BsonDocument();

            document.AddUniqueIdentifier(new Guid("20C2CAC4-C55D-4C5C-8937-33698A3EC6C7"));
            document.AddProperty("Name", "John Doe - Conditional");

            collection.InsertOne(document);
        }

        public override void Down()
        {
            var collection = GetCollection("Persons");
            var idFilter = Builders<BsonDocument>.Filter.Eq("_id", new Guid("20C2CAC4-C55D-4C5C-8937-33698A3EC6C7"));
            collection.DeleteOne(idFilter);
        }

        // Up condition
        public override bool ShouldUp()
        {
            return true;
        }

        // Down condition
        public override bool ShouldDown()
        {
            return false;
        }
    }
```

For more in-depth examples, check out the CSharpMongoMigrations.Demo project.


## How to launch migrations?
It’s actually quite simple: just create an instance of the *MigrationRunner* class.

```csharp
   var runner = new MigrationRunner("<mongoDb_connection_string>", "<database_name>", "<full_name_of_assembly_with_migrations>");
```
You can then call the Up or Down method to apply or revert migrations.

```csharp
   // Apply all migrations before specified version.
   // Use -1 as a version parameter to apply all existing migrations. ('-1' is a default parameter value)
   runner.Up("<version>"); 
   
   // Roll back all migrations after specified version.
   // Use -1 as a version parameter to downgrade all existing migrations. ('-1' is a default parameter value)
   runner.Down("<version>"); 
```

By default, these methods execute all defined migration types. To run migrations for a specific collection, use one of the available polymorphic overloads.
```csharp
   // Apply all migrations before specified version.
   // Use -1 as a version parameter to apply all existing migrations for the target collection. ('-1' is a default parameter value)
   runner.Up("<Collection_name>", "<version>"); 
   
   // Roll back all migrations after specified version.
   // Use -1 as a version parameter to downgrade all existing migrations for the target collection. ('-1' is a default parameter value)
   runner.Down("<Collection_name>", "<version>"); 
```
