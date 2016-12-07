# CSharpMongoMigrations

## What is it?

**CSharpMongoMigrations** is an alternative .NET library for mongo migrations. Despite of official package library allows both the *Up* and the *Down* data migrations. It's a big advantage for huge MongoDb projects. Moreover solution does not have any dependencies from outdated MongoDb driver. So you don't need to reference two different versions of Mongo driver.


## Installation


##### Nuget
```
Install-Package CSharpMongoMigrations
```

#### From Source
```
> git clone https://github.com/VladimirRybalko/CSharpMongoMigrations.git
```

## Documentation API
There are two types of migrations in solution.

1) **Migration**

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

Here we define the **Up** migration to added John Doe to *Person* collection. The **Down** method revert back the migration and restore database to the original state.

Take attention to **Migration** attribute. It's required to running migration by launcher. You should define the migration number (<span style="color:gray">'0' in example</span>) and description (<span style="color:gray">Add John Doe'</span>).
Note that each migration must have the unique number and arbitrary description.


2) **Document migration**

Such migrations allow to apply changes to each document in the specified collection.
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

You can find more detailed examples in *CSharpMongoMigrations.Demo* project.


## How to launch?
It's a really simple. You should create an instance of *MigrationRunner* class.

```csharp
   var runner = new MigrationRunner("<mongoDb_connection_string>", "<database_name>", "<location_to_assembly_with_migrations>");
```
Then you can call *Up* or *Down* method to apply or downgrade migrations.

```csharp
   // Apply all 'up' migrations before specified version.
   // Use -1 to apply all existing migrations. ('-1' is a default parameter value)
   runner.Up("<version>"); 
   
   // Apply all 'down' migrations after specified version.
   // Use -1 to downgrade all existing migrations. ('-1' is a default parameter value)
   runner.Down("<version>"); 
   
```