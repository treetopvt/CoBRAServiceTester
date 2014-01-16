using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Net.Http;
using WCFServiceTester.HelperClasses;

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


        #region "Commands"
        private RelayCommand _SendSensorCommand;

        /// <summary>
        /// Gets the SendSensorCommand.
        /// </summary>
        public RelayCommand SendSensorCommand
        {
            get
            {
                return _SendSensorCommand ?? (_SendSensorCommand = new RelayCommand(
                    ExecuteSendSensorCommand,
                    CanExecuteSendSensorCommand));
            }
        }

        private void ExecuteSendSensorCommand()
        {
            //will depend on what type of sensor is being selected.
            //initially just using simple sensor entry
            PostSimpleSensor();
        }

        private bool CanExecuteSendSensorCommand()
        {
            return EditSensor != null && EditSensor.IsValidSensor() && base.CanMakeServiceCall() && _ActiveProject !=null;
        }
        #endregion


        public async void PostSimpleSensor()
        {
            //?ProjectGUID={PROJECTGUID}&sensorName={SENSORNAME}&sensorGUID={SENSORGUID}&sensorType={SENSORTYPE}&Latitude={LATITUDE}&Longitude={LONGITUDE}
            //&IsEnabled={ISENABLED}&inAlarm={INALARM}&isFaulted={ISFAULTED}&ReadingLevel={READINGLEVEL}&ReadingUnits={READINGUNITS}&Agent={AGENT}&statusDescription={STATUSDESCRIPTION}
            //&ReadingID={READINGID}&TimeOfReading={TIMEOFREADING}
            var data = BuildBaseKeyValuePairs();
            data.AddRange<string, string>(EditSensor.BuildValuePairs());
            string result = await AuthenticatedPostData("CreateSensorEntry", "SensorService",data);
        }

        private Dictionary<string, string> BuildBaseKeyValuePairs()
        {
           var rtn = new Dictionary<string, string>();
            rtn.Add("ProjectGUID", _ActiveProject.ProjectGUID.ToString());
            rtn.Add("OrganizationName", _OrganizationName);
            rtn.Add("Latitude", Latitude);
            rtn.Add("Longitude", Longitude);
            return rtn;
        }
        
 
        #region Observable Properties


        /// <summary>
        /// The <see cref="Longitude" /> property's name.
        /// </summary>
        public const string LongitudePropertyName = "Longitude";

        private string _Longitude = "";

        /// <summary>
        /// Sets and gets the Longitude property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Longitude
        {
            get
            {
                return _Longitude;
            }
            set
            {
                Set(LongitudePropertyName, ref _Longitude, value);
            }
        }

        /// <summary>
        /// The <see cref="Latitude" /> property's name.
        /// </summary>
        public const string LatitudePropertyName = "Latitude";

        private string _Latitude = "";

        /// <summary>
        /// Sets and gets the Latitude property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Latitude
        {
            get
            {
                return _Latitude;
            }
            set
            {
                Set(LatitudePropertyName, ref _Latitude, value);
            }
        }


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