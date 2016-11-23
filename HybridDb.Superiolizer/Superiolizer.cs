using System;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace HybridDb.Superiolizer
{
    /// <summary>
    /// Implementation of HybridDB's <see cref="ISerializer"/> interface which relies on Newtonsoft JSON.NET to serialize/deserialize objects
    /// </summary>
    public class Superiolizer : ISerializer
    {
        readonly SuperiolizerConfiguration _configuration;
        readonly JsonSerializerSettings _settings;
        readonly Encoding _encoding;

        /// <summary>
        /// Creates the Superiolizer with default dettings
        /// </summary>
        public Superiolizer() : this(new SuperiolizerConfiguration(Encoding.UTF8))
        {
        }

        /// <summary>
        /// Creates the Superiolizer with the given configuration
        /// </summary>
        public Superiolizer(SuperiolizerConfiguration configuration)
        {
            _configuration = configuration;
            _encoding = configuration.Encoding;
            _settings = configuration.GetJsonSerializerSettings();
        }

        /// <summary>
        /// Gets the text encoding used by the Superiolizer
        /// </summary>
        public Encoding Encoding => _encoding;

        /// <summary>
        /// Serializes the given object to an array of bytes
        /// </summary>
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

        /// <summary>
        /// Deserializes the given array of bytes into the type specified
        /// </summary>
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
