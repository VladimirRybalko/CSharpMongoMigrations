# CSharpMongoMigrations

[![NuGet](https://img.shields.io/badge/nuget-2.2.0-blue.svg)](https://www.nuget.org/packages/CSharpMongoMigrations/)

## What is it?

**CSharpMongoMigrations** is an alternative .NET library for mongo migrations. Despite the official package the package allows the both *Up* and *Down* migrations. It might be a big advantage for the huge MongoDb projects. Moreover, the solution does not have any dependencies from the outdated MongoDb driver. So you don't need to reference different driver versions anymore.


## Documentation API
Generally, there are three types of migrations.

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

Here we define the **Up** migration to added John Doe to *Person* collection. The **Down** method roll back the migration and restore database to the original state.

Pay your attention to the **Migration** attribute. It's required for running migration by the launcher. You should define the unique migration number (<span style="color:gray">'0' in example</span>) and arbitrary description (<span style="color:gray">'Add John Doe'</span>).


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
Here, the *Migration* attribute defines the target collection name and the specific migration version for the predefined collection. It helps us to apply migration to a specific schema.


You can also find more detailed examples in the *CSharpMongoMigrations.Demo* project.


## How to launch migrations?
It's actually simple. You need to create an instance of the *MigrationRunner* class.

```csharp
   var runner = new MigrationRunner("<mongoDb_connection_string>", "<database_name>", "<full_name_of_assembly_with_migrations>");
```
Then you can call the *Up* or *Down* method to apply or downgrade migrations.

```csharp
   // Apply all migrations before specified version.
   // Use -1 as a version parameter to apply all existing migrations. ('-1' is a default parameter value)
   runner.Up("<version>"); 
   
   // Roll back all migrations after specified version.
   // Use -1 as a version parameter to downgrade all existing migrations. ('-1' is a default parameter value)
   runner.Down("<version>"); 
```

It's worth noting that above methods execute all types of migrations. To launch the collection specific migrations, please use one of the polymorphic methods.
```csharp
   // Apply all migrations before specified version.
   // Use -1 as a version parameter to apply all existing migrations for the target collection. ('-1' is a default parameter value)
   runner.Up("<Collection_name>", "<version>"); 
   
   // Roll back all migrations after specified version.
   // Use -1 as a version parameter to downgrade all existing migrations for the target collection. ('-1' is a default parameter value)
   runner.Down("<Collection_name>", "<version>"); 
```