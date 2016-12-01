# CSharpMongoMigrations

## What is it?

**CSharpMongoMigrations** is an alternative .NET library for mongo migrations. Despite of official package **CSharpMongoMigrations** allows both the *Up* and the *Down* data migrations. It's a big advantage for huge MongoDb projects. Moreover solution does not have any dependencies from outdated MongoDb driver. So you don't need to reference two different versions of Mongo driver.


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

Here we define the **Up** migration to added John Doe to *Person* collection. The **Down** method revert back migration and restore database to original state.

Take attention to **Migration** attribute. It's required to running migration by launcher. You should note the migration number (<span style="color:gray">'0' in example</span>) and description (<span style="color:gray">Add John Doe'</span>).
Note that each migration should have the unique number and arbitrary description.


2) **Document migration**
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

Such migrations allow to apply changes to each document in the specified collection.

Also you can find more detailed examples in *CSharpMongoMigrations.Demo* project.
