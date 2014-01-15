using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServiceTester.ViewModel
{
    public class AlertServiceViewModel:ServiceViewModelBase
    {
        public AlertServiceViewModel():base("AlertService", "", new Models.CredentialModel())
        {

        }

        public AlertServiceViewModel(string rootURL, Models.CredentialModel credentials)
            : base("AlertService", rootURL, credentials)
        {

        }

    }
}
