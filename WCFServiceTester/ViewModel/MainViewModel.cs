using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WCFServiceTester.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            _ServiceViewModels = new List<ServiceViewModelBase>() { new SensorServiceViewModel() };
            CurrentServiceView = _ServiceViewModels[0];
        }

        [Flags]
        public enum AvailableServices
        {
            [Description("Sensor Service")]
            SensorService = 0,
            [Description("Alert Service")]
            AlertService = 1
        }

        #region "Member Variables"

        private List<ServiceViewModelBase> _ServiceViewModels;

        #endregion


        #region "Helper Methods"

        private ViewModelBase GetViewModel(string serviceName)
        {
            var foundVM = _ServiceViewModels.FirstOrDefault(vm => vm.ServiceName == serviceName);
            if (foundVM == null)
            {
                switch (serviceName)
                {
                    case "AlertService":
                        foundVM = new AlertServiceViewModel();
                        break;
                    case "SensorService":
                        foundVM = new SensorServiceViewModel();
                        break;
                    default:
                        foundVM = null;
                        break;
                }

                if (foundVM != null)
                {//newly created, add to the collection
                    _ServiceViewModels.Add(foundVM);
                }
            }
            return foundVM;
        }

        #endregion


        #region "Observable Properties"

        /// <summary>
        /// The <see cref="ServerAddress" /> property's name.
        /// </summary>
        public const string ServerAddressPropertyName = "ServerAddress";

        private string _ServerAddress = "cobraserver14.defensegp.com";

        /// <summary>
        /// Sets and gets the ServerAddress property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ServerAddress
        {
            get
            {
                return _ServerAddress;
            }

            set
            {
                if (_ServerAddress == value)
                {
                    return;
                }

                RaisePropertyChanging(() => ServerAddress);
                _ServerAddress = value;
                RaisePropertyChanged(() => ServerAddress);
            }
        }

        /// <summary>
        /// The <see cref="ServiceUserName" /> property's name.
        /// </summary>
        public const string ServiceUserNamePropertyName = "ServiceUserName";

        private string _ServiceUserName = "serviceuser";

        /// <summary>
        /// Sets and gets the ServiceUserName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ServiceUserName
        {
            get
            {
                return _ServiceUserName;
            }

            set
            {
                if (_ServiceUserName == value)
                {
                    return;
                }

                RaisePropertyChanging(() => ServiceUserName);
                _ServiceUserName = value;
                RaisePropertyChanged(() => ServiceUserName);
            }
        }

        /// <summary>
        /// The <see cref="ServiceUserPassword" /> property's name.
        /// </summary>
        public const string ServiceUserPasswordPropertyName = "ServiceUserPassword";

        private string _ServiceUserPassword = "test";

        /// <summary>
        /// Sets and gets the ServiceUserPassword property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ServiceUserPassword
        {
            get
            {
                return _ServiceUserPassword;
            }

            set
            {
                if (_ServiceUserPassword == value)
                {
                    return;
                }

                RaisePropertyChanging(() => ServiceUserPassword);
                _ServiceUserPassword = value;
                RaisePropertyChanged(() => ServiceUserPassword);
            }
        }

        /// <summary>
        /// The <see cref="CurrentServiceView" /> property's name.
        /// </summary>
        public const string CurrentServiceViewPropertyName = "CurrentServiceView";

        private ViewModelBase _CurrentServiceView = null;

        /// <summary>
        /// Sets and gets the CurrentServiceView property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public ViewModelBase CurrentServiceView
        {
            get
            {
                return _CurrentServiceView;
            }

            set
            {
                if (_CurrentServiceView == value)
                {
                    return;
                }

                RaisePropertyChanging(CurrentServiceViewPropertyName);
                var oldValue = _CurrentServiceView;
                _CurrentServiceView = value;
                RaisePropertyChanged(CurrentServiceViewPropertyName, oldValue, value, true);
            }
        }

        #endregion


    }
}