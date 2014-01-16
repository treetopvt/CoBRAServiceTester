using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFServiceTester.Models
{
    public class SensorModel
    {
        public Guid SensorGUID { get; set; }//used for retrieval, will be ignored for creation
        public Guid StaticSensorGUID { get; set; }
        public string SensorName { get; set; }
        public string Description { get; set; }
        public string SensorType { get; set; }
        public string ReadingLevel { get; set; }
        public string ReadingUnits { get; set; }
        public string Agent { get; set; }
        public bool IsEnabled { get; set; }
        public bool InAlarm { get; set; }
        public bool IsFaulted { get; set; }
        public DateTime? TimeOfReading { get; set; }
        public string ReadingID { get; set; }
        public string StatusDescription { get; set; }
        public string SensorStatus { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateLastModified { get; set; }


        internal IEnumerable<KeyValuePair<string, string>> BuildValuePairs()
        {
            //?ProjectGUID={PROJECTGUID}&sensorName={SENSORNAME}&sensorGUID={SENSORGUID}&sensorType={SENSORTYPE}&Latitude={LATITUDE}&Longitude={LONGITUDE}
            //&IsEnabled={ISENABLED}&inAlarm={INALARM}&isFaulted={ISFAULTED}&ReadingLevel={READINGLEVEL}&ReadingUnits={READINGUNITS}&Agent={AGENT}&statusDescription={STATUSDESCRIPTION}
            //&ReadingID={READINGID}&TimeOfReading={TIMEOFREADING}
            var rtnDict = new Dictionary<string, string>();
            rtnDict.Add("SensorName", SensorName);
            rtnDict.Add("SensorGuid", StaticSensorGUID.ToString());
            rtnDict.Add("SensorType", SensorType);
            rtnDict.Add("InAlarm", InAlarm.ToString());
            rtnDict.Add("IsFaulted", IsFaulted.ToString());
            rtnDict.Add("ReadingLevel", ReadingLevel);
            rtnDict.Add("ReadingUnits", ReadingUnits);
            rtnDict.Add("ReadingID", ReadingID);
            rtnDict.Add("TimeOfReading", TimeOfReading.HasValue?TimeOfReading.Value.ToString():"");
            rtnDict.Add("Agent", Agent);
            rtnDict.Add("StatusDescription", StatusDescription);
            rtnDict.Add("SensorStatus", SensorStatus);

            return rtnDict;
        }

        internal bool IsValidSensor()
        {
            //validate sensor here.
            //very least needs name and type?
            return !string.IsNullOrEmpty(SensorName);
        }
    }
}
