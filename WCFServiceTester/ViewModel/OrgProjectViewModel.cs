﻿using GalaSoft.MvvmLight.Command;
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
        public OrgProjectViewModel():base("OrgProjectService", "", new Models.CredentialModel())
        {
            //don't want message from myself
            Messenger.Default.Unregister <NotificationMessage<Models.OrgProjectModel>>(this);
            SendOrgProjectChangeMessage("OrganizationName");
        }
        public OrgProjectViewModel(string rootURL, Models.CredentialModel credentials)
            : base("OrgProjectService", rootURL, credentials)
        {
            //don't want message from myself
            Messenger.Default.Unregister < NotificationMessage<Models.OrgProjectModel>>(this);
            SendOrgProjectChangeMessage("OrganizationName");
        }


        private void SendOrgProjectChangeMessage(string valueUpdated)
        {
            var message = new NotificationMessage<Models.OrgProjectModel>(new Models.OrgProjectModel() { OrganizationName = OrganizationName, ActiveProject = SelectedProject }, valueUpdated);
            Messenger.Default.Send(message);
        }

        #region "Observable Properties"

        /// <summary>
        /// The <see cref="OrganizationName" /> property's name.
        /// </summary>
        public const string OrganizationNamePropertyName = "OrganizationName";

        private string _OrganizationName = "TomDev";

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
                SendOrgProjectChangeMessage("ActiveProject");
            }
        }

        /// <summary>
        /// The <see cref="SelectedProject" /> property's name.
        /// </summary>
        public const string SelectedProjectPropertyName = "SelectedProject";

        private Models.ProjectModel _SelectedProject = null;

        /// <summary>
        /// Sets and gets the SelectedProject property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public Models.ProjectModel SelectedProject
        {
            get
            {
                return _SelectedProject;
            }

            set
            {
                if (_SelectedProject == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedProjectPropertyName);
                var oldValue = _SelectedProject;
                _SelectedProject = value;
                RaisePropertyChanged(SelectedProjectPropertyName, oldValue, value, true);
                SendOrgProjectChangeMessage("ActiveProject");
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
            SendStatus("Retrieving Projects",0, true);
            //GetProjectList?OrganizationName={ORGANIZATIONNAME}&SinceDate={SINCEDATE}
            var data = new Dictionary<string,string>();
            data.Add("OrganizationName", OrganizationName);
            data.Add("SinceDate", new DateTime(2012, 01, 01).ToShortDateString());
            
            string result = await AuthenticatedGetData("GetProjectList", "ProjectService", data);
            //handle fail state with message/status update
            ProjectList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.ProjectModel>>(result);
            SendStatus(String.Format("{0} Projects Retrieved", ProjectList.Count));
        }
    }
}
