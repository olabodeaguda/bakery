using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeryPR.Models
{
    public class LoginModel
    {
        private string _username;

        public string username
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _status;

        public string status
        {
            get { return _status; }
            set { _status = value; }
        }

        private string _pwd;

        public string pwd
        {
            get { return _pwd; }
            set { _pwd = value; }
        }

    }
}
