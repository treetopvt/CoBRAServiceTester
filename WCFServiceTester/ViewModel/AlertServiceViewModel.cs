using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServiceTester.ViewModel
{
    public class AlertServiceViewModel:ServiceViewModelBase
    {
        public AlertServiceViewModel():base("AlertService", "", "", "")
        {

        }

        public AlertServiceViewModel(string rootURL, string userName, string password)
            : base("AlertService", rootURL, userName, password)
        {

        }

    }
}
