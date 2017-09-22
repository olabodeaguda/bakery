using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Utilities
{
    public class AppConfigUtils
    {
        public static void Write(string fieldName, string value)
        {
            Configuration oConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);//
            oConfig.AppSettings.Settings[fieldName].Value = value;
            oConfig.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        public static string Read(string nm)
        {
            Configuration oConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string mf = oConfig.AppSettings.Settings[nm].Value;
            return mf;
        }
    }
}
