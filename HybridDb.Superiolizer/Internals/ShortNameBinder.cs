using System;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;

namespace HybridDb.Superiolizer.Internals
{
    class ShortNameBinder : DefaultSerializationBinder
    {
        const string PseudoAssemblyName = "[SPRLZR]";
        readonly Dictionary<Type, string> _typeToName = new Dictionary<Type, string>();
        readonly Dictionary<string, Type> _nameToType = new Dictionary<string, Type>();

        public ShortNameBinder(Dictionary<Type, string> names)
        {
            foreach (var kvp in names)
            {
                _typeToName[kvp.Key] = kvp.Value;
                _nameToType[kvp.Value] = kvp.Key;
            }
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            string name;

            if (_typeToName.TryGetValue(serializedType, out name))
            {
                assemblyName = PseudoAssemblyName;
                typeName = name;
                return;
            }

            base.BindToName(serializedType, out assemblyName, out typeName);
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            if (assemblyName == PseudoAssemblyName)
            {
                try
                {
                    return _nameToType[typeName];
                }
                catch (Exception exception)
                {
                    throw new ArgumentException($"Could not get type from short-name {typeName}", exception);
                }
            }

            return base.BindToType(assemblyName, typeName);
        }
    }
}