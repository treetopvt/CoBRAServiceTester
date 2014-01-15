using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServiceTester.Models
{
    public class ProjectModel
    {
        public Guid ProjectGUID { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string ProjectType { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastModified { get; set; }
    }
}
