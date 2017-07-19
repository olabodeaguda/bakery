using BakeryPR.Models;
using BakeryPR.Utilities;
using System;
using System.Configuration;
using System.Xml;

namespace BakeryPR.DAO
{
    public class AppConfigDao
    {
        public void updateConfig(LoginModel lm)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["username"].Value = lm.username;
                config.AppSettings.Settings["fullname"].Value = lm.fullname;
                config.AppSettings.Settings["status"].Value = lm.status;
                config.AppSettings.Settings["isLogin"].Value = lm.isLogin;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception x)
            {
                throw new Exception("An error occure while trying to update user section");
            }
        }

        public LoginModel read()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            LoginModel loginM = new LoginModel();
            loginM.username = config.AppSettings.Settings["username"]?.Value;
            loginM.fullname = config.AppSettings.Settings["fullname"]?.Value;
            loginM.status = config.AppSettings.Settings["status"]?.Value;
            loginM.isLogin = config.AppSettings.Settings["isLogin"]?.Value;

            return loginM;
        }

    }
}
