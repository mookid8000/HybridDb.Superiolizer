﻿using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace HybridDb.Superiolizer
{
    public class Superiolizer : ISerializer
    {
        readonly JsonSerializerSettings _settings;
        readonly Encoding _encoding;

        public Superiolizer(Encoding encoding)
        {
            _encoding = encoding;
            _settings = new JsonSerializerSettings { };
        }

        public byte[] Serialize(object obj)
        {
            try
            {
                var jsonString = JsonConvert.SerializeObject(obj, _settings);

                return _encoding.GetBytes(jsonString);
            }
            catch (Exception exception)
            {
                throw new SerializationException($"Could not serialize {obj}", exception);
            }
        }

        public object Deserialize(byte[] data, Type type)
        {
            var jsonString = _encoding.GetString(data);

            try
            {
                var obj = JsonConvert.DeserializeObject(jsonString, type, _settings);

                return obj;
            }
            catch (Exception exception)
            {
                throw new SerializationException($"Could not deserialize JSON string {jsonString}", exception);
            }
        }
    }
}
