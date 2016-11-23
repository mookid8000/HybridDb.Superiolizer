using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace HybridDb.Superiolizer
{
    public class Configuration
    {
        readonly Dictionary<Type, StringFormatter> _customStringFormatters = new Dictionary<Type, StringFormatter>();

        public Encoding Encoding { get; }

        public Configuration(Encoding encoding)
        {
            Encoding = encoding;
        }

        public JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                Converters = _customStringFormatters.Values
                    .Select(formatter => formatter.ToConverter())
                    .ToList(),

                TypeNameHandling = TypeNameHandling.Objects
            };
        }

        public void AddCustomSerialization<T>(Func<T, string> serializer, Func<string, T> deserializer)
        {
            var type = typeof(T);

            if (_customStringFormatters.ContainsKey(type))
            {
                throw new InvalidOperationException($"Cannot add custom serialiation for {type} because one has already been registered - currently have the following formatters: {string.Join(", ", _customStringFormatters.Values.Select(f => f.Type))}");
            }

            _customStringFormatters[type] = new StringFormatter<T>(serializer, deserializer);
        }

        abstract class StringFormatter
        {
            public abstract Type Type { get; }
            public abstract JsonConverter ToConverter();
        }

        class StringFormatter<T> : StringFormatter
        {
            public StringFormatter(Func<T, string> serializer, Func<string, T> deserializer)
            {
                Serializer = serializer;
                Deserializer = deserializer;
            }

            public Func<T, string> Serializer { get; }

            public Func<string, T> Deserializer { get; }

            public override Type Type => typeof(T);

            public override JsonConverter ToConverter()
            {
                return new StringFormatterConverter(Serializer, Deserializer);
            }

            class StringFormatterConverter : JsonConverter
            {
                readonly Func<T, string> _serializer;
                readonly Func<string, T> _deserializer;

                public StringFormatterConverter(Func<T, string> serializer, Func<string, T> deserializer)
                {
                    _serializer = serializer;
                    _deserializer = deserializer;
                }

                public override bool CanConvert(Type objectType)
                {
                    return objectType == typeof(T);
                }

                public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
                {
                    if (value == null)
                    {
                        writer.WriteNull();
                        return;
                    }

                    var t = (T)value;

                    writer.WriteValue(_serializer(t));
                }

                public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
                {
                    var tokenType = reader.TokenType;

                    if (tokenType == JsonToken.Null) return null;

                    if (tokenType != JsonToken.String)
                    {
                        throw new SerializationException($"Expected either NULL or STRING token type - got {tokenType}");
                    }

                    var stringValue = (string)reader.Value;

                    return _deserializer(stringValue);
                }
            }
        }
    }
}