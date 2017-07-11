using BakeryPR.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.DAO
{
    public class AppConfigDao
    {
        public LoginModel read()
        {
            var connectionManagerDatabaseServers = ConfigurationManager.GetSection("LoginModel") as NameValueCollection;
            if (connectionManagerDatabaseServers != null)
            {
                LoginModel loginM = new LoginModel();
                foreach (string serverKey in connectionManagerDatabaseServers.AllKeys)
                {
                    string serverValue = connectionManagerDatabaseServers.GetValues(serverKey).FirstOrDefault();

                    switch (serverKey)
                    {
                        case "username":
                            loginM.username = serverValue;
                            break;
                        case "status":
                            loginM.status = serverValue;
                            break;
                        case "pwd":
                            loginM.pwd = serverValue;
                            break;
                        default:
                            break;
                    }
                }

                return loginM;
            }

            return null;
        }

    }
}
