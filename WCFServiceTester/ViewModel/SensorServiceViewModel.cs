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
            : base("SensorService", "", new Models.CredentialModel())
        {
            EditSensor = new Models.SensorModel();
        }

        public SensorServiceViewModel(string rootURL, Models.CredentialModel credentials)
            : base("SensorService", rootURL, credentials)
        {
            EditSensor = new Models.SensorModel();
        }


        public async void PostSimpleSensor()
        {
            //?ProjectGUID={PROJECTGUID}&sensorName={SENSORNAME}&sensorGUID={SENSORGUID}&sensorType={SENSORTYPE}&Latitude={LATITUDE}&Longitude={LONGITUDE}
            //&IsEnabled={ISENABLED}&inAlarm={INALARM}&isFaulted={ISFAULTED}&ReadingLevel={READINGLEVEL}&ReadingUnits={READINGUNITS}&Agent={AGENT}&statusDescription={STATUSDESCRIPTION}
            //&ReadingID={READINGID}&TimeOfReading={TIMEOFREADING}
            var data = BuildBaseKeyValuePairs();
            string result = await AuthenticatedGetData("CreateSensorEntry", "SensorService",data);
        }

        private Dictionary<string, string> BuildBaseKeyValuePairs()
        {
           var rtn = new Dictionary<string, string>();
            rtn.Add("ProjectGUID", Guid.NewGuid().ToString());
            return rtn;
        }
        
 
        #region Observable Properties
        /// <summary>
        /// The <see cref="EditSensor" /> property's name.
        /// </summary>
        public const string EditSensorPropertyName = "EditSensor";

        private Models.SensorModel _myEditSensor = null;

        /// <summary>
        /// Sets and gets the EditSensor property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public Models.SensorModel EditSensor
        {
            get
            {
                return _myEditSensor;
            }
            set
            {
                Set(EditSensorPropertyName, ref _myEditSensor, value, true);
            }
        }

        #endregion

    }
}