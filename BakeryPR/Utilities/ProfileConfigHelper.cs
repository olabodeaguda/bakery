using System.Configuration;

namespace BakeryPR.Utilities
{
    public class ProfileConfigHelper : ConfigurationSection
    {
        public const string SectionName = "Profile";

        private const string EndpointCollectionName = "properties";

        [ConfigurationProperty(EndpointCollectionName)]
        [ConfigurationCollection(typeof(ProfileKeyCollection), AddItemName = "add")]
        public ProfileKeyCollection ProfileEndpoints
        {
            get
            {
                return (ProfileKeyCollection)base[EndpointCollectionName];
            }
        }
    }
}
