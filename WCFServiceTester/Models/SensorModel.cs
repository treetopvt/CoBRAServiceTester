using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServiceTester.Models
{
    public class SensorModel
    {
        public Guid SensorGUID { get; set; }
        public string SensorName { get; set; }
        public string Description { get; set; }
        public string SensorType { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastModified { get; set; }

    }
}
