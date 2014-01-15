using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WCFServiceTester.HelperClasses;
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
            SubscribeToMessages();
            _ServiceViewModels = new List<ServiceViewModelBase>() {};
            SelectedService = AvailableServicesEnum.OrgProjectService.ToString(true);
            
            
            _AvailableServices =  GetAvailableServicesList();
        }

        public enum AvailableServicesEnum
        {
            [Description(@"Organization/Project Service")]
            OrgProjectService = 0,
            [Description("Sensor Service")]
            SensorService = 1,
            [Description("Alert Service")]
            AlertService = 2
        }

        #region "Member Variables"

        private List<ServiceViewModelBase> _ServiceViewModels;

        #endregion


        #region "Helper Methods"

        private void SubscribeToMessages()
        {
            Messenger.Default.Register<NotificationMessage<Models.OrgProjectModel>>(this, updateOrgProject);
        }

        private void SendUpdateMessages()
        {
            var message = new NotificationMessage<Models.CredentialModel>(new Models.CredentialModel(ServiceUserName, ServiceUserPassword, ImpersonateUserName),"CredentialsUpdated");
            Messenger.Default.Send(message);

        }

        private void updateOrgProject(NotificationMessage<Models.OrgProjectModel> msg)
        {
            this.OrganizationName = msg.Content.OrganizationName;
            this.ProjectName = msg.Content.ProjectName;
        }

        private List<String> GetAvailableServicesList()
        {
            var rtn = new List<String>();
            foreach (var item in Enum.GetValues(typeof(AvailableServicesEnum)).Cast<AvailableServicesEnum>())
            {
                rtn.Add(item.ToString(true));
                //item.ToString(true);
            }
            return rtn;
        }

        private ViewModelBase GetViewModel(string serviceName)
        {
            var serviceEnum = Enum.GetValues(typeof(AvailableServicesEnum)).Cast<AvailableServicesEnum>().FirstOrDefault(a => a.ToString(true) == serviceName);
            var foundVM = _ServiceViewModels.FirstOrDefault(vm => vm.ServiceName == serviceEnum.ToString(false));
            if (foundVM == null)
            {
                var credentials = new Models.CredentialModel(this.ServiceUserName, this.ServiceUserPassword, this.ImpersonateUserName);
                switch (serviceEnum)
                {
                    case AvailableServicesEnum.OrgProjectService:
                        foundVM = new OrgProjectViewModel(this._ServerAddress, credentials);
                        break;
                    case AvailableServicesEnum.AlertService:
                        foundVM = new AlertServiceViewModel(this._ServerAddress, credentials);
                        break;
                    case AvailableServicesEnum.SensorService:
                        foundVM = new SensorServiceViewModel(this._ServerAddress, credentials);
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

        private string _ServerAddress = "cobraserver9.defensegp.com";

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
                var message = new NotificationMessage<string>(_ServerAddress, "ServerUpdated");
                Messenger.Default.Send(message);

            }
        }

        /// <summary>
        /// The <see cref="ServiceUserName" /> property's name.
        /// </summary>
        public const string ServiceUserNamePropertyName = "ServiceUserName";

        private string _ServiceUserName = "service@tomdev";

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
                
                if (_ServiceUserName !=value && !String.IsNullOrEmpty(value)){
                    RaisePropertyChanging(() => ServiceUserName);
                    _ServiceUserName = value;
                    RaisePropertyChanged(() => ServiceUserName);
                    SendUpdateMessages();
                }
            }
        }

        /// <summary>
        /// The <see cref="ServiceUserPassword" /> property's name.
        /// </summary>
        public const string ServiceUserPasswordPropertyName = "ServiceUserPassword";

        private string _ServiceUserPassword = "service";

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
                SendUpdateMessages();
            }
        }
        /// <summary>
        /// The <see cref="ImpersonateUserName" /> property's name.
        /// </summary>
        public const string ImpersonateUserNamePropertyName = "ImpersonateUserName";

        private string _ImpersonateUserName = "Tom@TomDev";

        /// <summary>
        /// Sets and gets the ImpersonateUserName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string ImpersonateUserName
        {
            get
            {
                return _ImpersonateUserName;
            }

            set
            {
                if (_ImpersonateUserName == value)
                {
                    return;
                }

                RaisePropertyChanging(ImpersonateUserNamePropertyName);
                var oldValue = _ImpersonateUserName;
                _ImpersonateUserName = value;
                RaisePropertyChanged(ImpersonateUserNamePropertyName, oldValue, value, true);
                SendUpdateMessages();
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

        /// <summary>
        /// The <see cref="AvailableServices" /> property's name.
        /// </summary>
        public const string AvailableServicesPropertyName = "AvailableServices";

        private List<String> _AvailableServices = new List<String>();

        /// <summary>
        /// Sets and gets the AvailableServices property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public List<String> AvailableServices
        {
            get
            {
                return _AvailableServices;
            }

            set
            {
                if (_AvailableServices == value)
                {
                    return;
                }

                RaisePropertyChanging(AvailableServicesPropertyName);
                var oldValue = _AvailableServices;
                _AvailableServices = value;
                RaisePropertyChanged(AvailableServicesPropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// The <see cref="SelectedService" /> property's name.
        /// </summary>
        public const string SelectedServicePropertyName = "SelectedService";

        private string _SelectedService = "";

        /// <summary>
        /// Sets and gets the SelectedService property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string SelectedService
        {
            get
            {
                return _SelectedService;
            }
            set
            {
                Set(SelectedServicePropertyName, ref _SelectedService, value, true);
                CurrentServiceView = GetViewModel(_SelectedService);
            }
        }

        /// <summary>
        /// The <see cref="OrganizationName" /> property's name.
        /// </summary>
        public const string OrganizationNamePropertyName = "OrganizationName";

        private string _OrganizationName = "";

        /// <summary>
        /// Sets and gets the OrganizationName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string OrganizationName
        {
            get
            {
                return _OrganizationName;
            }

            set
            {
                if (_OrganizationName == value)
                {
                    return;
                }

                RaisePropertyChanging(OrganizationNamePropertyName);
                var oldValue = _OrganizationName;
                _OrganizationName = value;
                RaisePropertyChanged(OrganizationNamePropertyName, oldValue, value, true);
            }
        }

        /// <summary>
        /// The <see cref="ProjectName" /> property's name.
        /// </summary>
        public const string ProjectNamePropertyName = "ProjectName";

        private string _ProjectName = "";

        /// <summary>
        /// Sets and gets the ProjectName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string ProjectName
        {
            get
            {
                return _ProjectName;
            }

            set
            {
                if (_ProjectName == value)
                {
                    return;
                }

                RaisePropertyChanging(ProjectNamePropertyName);
                var oldValue = _ProjectName;
                _ProjectName = value;
                RaisePropertyChanged(ProjectNamePropertyName, oldValue, value, true);
            }
        }

        #endregion


    }
}