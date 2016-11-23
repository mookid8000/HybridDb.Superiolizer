using System;
using System.Text;
using HybridDb.Config;

namespace HybridDb.Superiolizer
{
    /// <summary>
    /// Configuration extensions that make it convenient to configure Superiolizer
    /// </summary>
    public static class HybridDbConfigurationExtensions
    {
        /// <summary>
        /// Enables Superiolizer
        /// </summary>
        public static void UseSuperiolier(this Configuration configuration, Action<SuperiolizerConfiguration> configurationCallback = null)
        {
            var superiolizerConfiguration = new SuperiolizerConfiguration(Encoding.UTF8);

            configurationCallback?.Invoke(superiolizerConfiguration);

            configuration.UseSerializer(new Superiolizer(superiolizerConfiguration));
        }
    }
}