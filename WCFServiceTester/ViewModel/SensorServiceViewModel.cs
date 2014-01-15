using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace WCFServiceTester.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SensorServiceViewModel : ServiceViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the SensorServiceViewModel class.
        /// </summary>
        public SensorServiceViewModel()
            : base("SensorService", "", "", "")
        {
        }

        public SensorServiceViewModel(string rootURL, string userName, string password)
            : base("SensorService", rootURL, userName, password)
        {

        }


        public async void PostSimpleSensor()
        {
            //?ProjectGUID={PROJECTGUID}&sensorName={SENSORNAME}&sensorGUID={SENSORGUID}&sensorType={SENSORTYPE}&Latitude={LATITUDE}&Longitude={LONGITUDE}
            //&IsEnabled={ISENABLED}&inAlarm={INALARM}&isFaulted={ISFAULTED}&ReadingLevel={READINGLEVEL}&ReadingUnits={READINGUNITS}&Agent={AGENT}&statusDescription={STATUSDESCRIPTION}
            //&ReadingID={READINGID}&TimeOfReading={TIMEOFREADING}
            var data = BuildBaseKeyValuePairs();
            string result = await AuthenticatedGetData("CreateSensorEntry", "SensorService",new FormUrlEncodedContent(data));
        }

        private Dictionary<string, string> BuildBaseKeyValuePairs()
        {
           var rtn = new Dictionary<string, string>();
            rtn.Add("ProjectGUID", Guid.NewGuid().ToString());
            return rtn;
        }
    }
}