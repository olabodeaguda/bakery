using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Utilities
{
    [ConfigurationCollection(typeof(ProfileElement))]
    public class ProfileKeyCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ProfileElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProfileElement)(element)).Key;
        }

        public ProfileElement this[int idx]
        {
            get
            {
                return (ProfileElement)BaseGet(idx);
            }
        }
    }
}
