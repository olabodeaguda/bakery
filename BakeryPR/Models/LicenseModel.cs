using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class LicenseModel
    {
        public int id { get; set; }
        public string value { get; set; }
        public string key { get; set; }
        public string appVersion { get; set; }
        public string appName { get; set; }
        public bool isUsed { get; set; }
        public int loadCount { get; set; }
        public string hostName { get; set; }
    }
}
