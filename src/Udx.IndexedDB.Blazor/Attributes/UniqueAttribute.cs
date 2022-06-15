using System;

namespace Udx.IndexedDB.Blazor.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class UniqueAttribute : Attribute { }
}
