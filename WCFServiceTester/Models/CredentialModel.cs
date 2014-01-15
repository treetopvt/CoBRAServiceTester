using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServiceTester.Models
{
    public class CredentialModel
    {
        public CredentialModel()
        {

        }
        public CredentialModel(string userName, string password, string impersonateUser = "")
        {
            UserName = userName;
            Password = password;
            ImpersonateUserName = impersonateUser;
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ImpersonateUserName { get; set; }
    }
}
