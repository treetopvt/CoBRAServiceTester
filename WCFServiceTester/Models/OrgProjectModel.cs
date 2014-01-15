using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServiceTester.Models
{
    public class OrgProjectModel
    {
        public string OrganizationName { get; set; }
        public string ProjectName { get; set; }
        public Guid OrganizationGUID { get; set; }
        public Guid ProjectGUID { get; set; }

    }
}
