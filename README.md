**UdxFramework

# Udx.IndexedDB.Blazor

An easy way to interact with IndexedDB and make it feel like EF Core but `async`.

## NuGet installation
The NuGet package is at: https://www.nuget.org/packages/Udx.IndexedDB.Blazor

Either install it from command line:
```powershell
PM> Install-Package Udx.IndexedDB.Blazor
```


## Features
- Connect and create database
- Add record
- Remove record
- Edit record
- Search record

## How to use
1. Add `TG.Blazor.IndexedDB/indexedDb.Blazor.js` to your `index.html`
```html
<script src="_content/TG.Blazor.IndexedDB/indexedDb.Blazor.js"></script>
```
2. Register `IndexedDbFactory` as a service.
```CSharp
services.AddSingleton<IIndexedDbFactory, IndexedDbFactory>();
```
- `IIndexedDbFactory` is used to create the database connection and will create the database instance for you.

- `IndexedDbFactory` requires an instance of `IJSRuntime` which should normally already be registered.

3. Create any code first database model and inherit from `IndexedDb`. Only properties with the type `IndexedSet<>` will be used, any other properties will be ignored.
```CSharp
 public class ExampleDb : Udx.IndexedDB.Blazor.IndexedDb
    {
        public ExampleDb(IJSRuntime jSRuntime, string name, int version) : base(jSRuntime, name, version) { }

        public IndexedSet<Person> People { get; set; }
        public IndexedSet<Dog> Dog { get; set; }
    }
```
- Your model (eg. `Person`) should contain an `Id` property or a property marked with the `Key` attribute.
```CSharp
 public class Person
    {
        [Key]
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [ForeignKey(nameof(DogId))]
        [Browsable(false)]
        public Dog Dog { get; set; } = new Dog()
        {
            Id = 10,
            Name = "IndexedDB"
        };

        public long DogId { get; set; }
    }

    public class Dog
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
```

4. Now you can start using your database.

- Usage in Razor via inject: `@inject IIndexedDbFactory DbFactory`

### Adding records
```CSharp
using (var db = await this.DbFactory.Create<ExampleDb>())
        {
            for (int i = 0; i < 10; i++)
            {
                var p = new Person()
                    {
                        FirstName = Guid.NewGuid().ToString(),
                        LastName = Guid.NewGuid().ToString(),
                        DogId = i,
                        Dog = new Dog
                        {
                            Id = i,
                            Name = $"dog {i}"
                        }

                    };
                db.People.Add(p);
                db.Dog.Add(p.Dog);

            }
            await db.SaveChanges();
        }
```
### Removing records
To remove an element it is faster to use an already created reference. You should also be able to remove an object only by it's `Id` but you have to use the `.Remove(object)` method (eg. `.Remove(new object() { Id = 1 })`)
```CSharp
using (var db = await this.DbFactory.Create<ExampleDb>())
{
  var firstPerson = db.People.First();
  db.People.Remove(firstPerson);
  await db.SaveChanges();
}
```
### Modifying records
```CSharp
using (var db = await this.DbFactory.Create<ExampleDb>())
{
  var personWithId1 = db.People.Single(x => x.Id == 1);
  personWithId1.FirstName = "This is 100% a first name";
  await db.SaveChanges();
}
```
### Search records
```CSharp
  using (var db = await this.DbFactory.Create<ExampleDb>())
        {
            listPeople = from p in db.People
                         join d in db.Dog on p.DogId equals d.Id
                         where d.Name.Contains("dog")
                         select new Person
                             {
                                 Id = p.Id,
                                 FirstName = p.FirstName,
                                 LastName = p.LastName,
                                 DogId = p.DogId,
                                 Dog = d
                             };
        }
```

## License
Original [license](https://github.com/Jinjinov/IndexedDB.Blazor/blob/master/LICENSE).
Original [license](https://github.com/wtulloch/Blazor.IndexedDB/blob/master/LICENSE).

## Site
Udx [Udx.com.cn](http://www.udx.com.cn)

Licensed under the [MIT](LICENSE) license.