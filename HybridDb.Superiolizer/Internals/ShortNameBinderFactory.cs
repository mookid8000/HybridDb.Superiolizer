using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace HybridDb.Superiolizer.Internals
{
    class ShortNameBinderFactory
    {
        readonly Dictionary<Type, string> _names = new Dictionary<Type, string>();

        public void Add<T>(string name)
        {
            var type = typeof(T);

            if (_names.ContainsKey(type))
            {
                throw new ArgumentException($@"Cannot add short-name for {type} because one has already been added - currently have the following registrations:

{string.Join(Environment.NewLine, _names.Select(kvp => $"    {kvp.Value}: {kvp.Key}"))}
");
            }

            if (_names.ContainsValue(name))
            {
                throw new ArgumentException($@"Cannot add short-name {name} because one has already been added - currently have the following registrations:

{string.Join(Environment.NewLine, _names.Select(kvp => $"    {kvp.Value}: {kvp.Key}"))}
");
            }

            _names[type] = name;
        }

        public SerializationBinder CreateBinder()
        {
            return new ShortNameBinder(_names);
        }
    }
}