using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WCFServiceTester.Models;

namespace WCFServiceTester.ViewModel
{
    public class OrgProjectViewModel:ServiceViewModelBase
    {
        public OrgProjectViewModel():base("OrgProjectService", "", "", "")
        {
            //don't want message from myself
            Messenger.Default.Unregister <NotificationMessage<Models.OrgProjectModel>>(this);

        }
        public OrgProjectViewModel(string rootURL, string userName, string password)
            : base("OrgProjectService", rootURL, userName, password)
        {
            //don't want message from myself
            Messenger.Default.Unregister < NotificationMessage<Models.OrgProjectModel>>(this);
        }


        private void SendOrgProjectChangeMessage(string valueUpdated)
        {
            var message = new NotificationMessage<Models.OrgProjectModel>(new Models.OrgProjectModel() { OrganizationName = OrganizationName }, valueUpdated);
            Messenger.Default.Send(message);
        }

        #region "Observable Properties"

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
                SendOrgProjectChangeMessage("OrganizationName");
            }
        }

        /// <summary>
        /// The <see cref="ProjectList" /> property's name.
        /// </summary>
        public const string ProjectListPropertyName = "ProjectList";

        private List<ProjectModel> _ProjectList = null;

        /// <summary>
        /// Sets and gets the ProjectList property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<ProjectModel> ProjectList
        {
            get
            {
                return _ProjectList;
            }

            set
            {
                if (_ProjectList == value)
                {
                    return;
                }

                RaisePropertyChanging(ProjectListPropertyName);
                _ProjectList = value;
                RaisePropertyChanged(ProjectListPropertyName);
            }
        }
        #endregion

        #region "Commands"

        private RelayCommand _GetProjectListCommand;

        /// <summary>
        /// Gets the GetProjectList.
        /// </summary>
        public RelayCommand GetProjectListCommand
        {
            get
            {
                return _GetProjectListCommand
                    ?? (_GetProjectListCommand = new RelayCommand(
                                          () =>
                                          {
                                              GetProjects();
                                          },
                                          () => (!String.IsNullOrEmpty(OrganizationName) && CanMakeServiceCall())));
            }
        }

        #endregion

        private async void GetProjects()
        {
            //GetProjectList?OrganizationName={ORGANIZATIONNAME}&SinceDate={SINCEDATE}
            var data = new Dictionary<string,string>();
            data.Add("ORGANIZATIONNAME", OrganizationName);
            //data.Add("SINCE")
            string result = await AuthenticatedGetData("GetProjectList", "ProjectService",new FormUrlEncodedContent(data));
        }
    }
}
