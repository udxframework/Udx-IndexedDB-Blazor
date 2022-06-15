using Microsoft.JSInterop;
using Udx.IndexedDB.Blazor;

namespace Udx.IndexedDB.Blazor.Example.Models
{
    public class ExampleDb : Udx.IndexedDB.Blazor.IndexedDb
    {
        public ExampleDb(IJSRuntime jSRuntime, string name, int version) : base(jSRuntime, name, version) { }

        public IndexedSet<Person> People { get; set; }
        public IndexedSet<Dog> Dog { get; set; }
    }
}
