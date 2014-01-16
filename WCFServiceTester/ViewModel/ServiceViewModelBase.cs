using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;

namespace WCFServiceTester.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public abstract class ServiceViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the ServiceViewModelBase class.
        /// </summary>
        public ServiceViewModelBase(string serviceName, string rootURL, Models.CredentialModel credentials)
        {
            ServiceName = serviceName;
            _rootURL = rootURL;
            this.Credientials = credentials;
            RegisterMessaging();
        }


        public string ServiceName { get; set; }
        
        private string _rootURL = "";

        protected Models.ProjectModel _ActiveProject = null;
        protected string _OrganizationName = "";

        public Models.CredentialModel Credientials { get; set; }

        internal void RegisterMessaging()
        {
            Messenger.Default.Register<NotificationMessage<Models.CredentialModel>>
    (this, updateCredentials);
            Messenger.Default.Register<NotificationMessage<string>>
    (this, updateServer );
            Messenger.Default.Register<NotificationMessage<Models.OrgProjectModel>>
(this, updateOrgProject);
        }

        private void updateOrgProject(NotificationMessage<Models.OrgProjectModel> msg)
        {
            _OrganizationName = msg.Content.OrganizationName;
            if (msg.Content.ActiveProject != null)
            {
                _ActiveProject = msg.Content.ActiveProject;
            }
        }

        private void updateCredentials(NotificationMessage<Models.CredentialModel> msg)
        {
            this.Credientials = msg.Content;
        }

        private void updateServer(NotificationMessage<string> msg){
            if (msg.Notification == "ServerUpdated"){
                _rootURL = msg.Content;
            }
        }

        internal AuthenticationHeaderValue GetAuthHeader()
        {
            var credString = string.Format(@"{0}:{1}:{2}", Credientials.UserName, Credientials.Password, Credientials.ImpersonateUserName);
            credString = credString.TrimEnd(':');
            return new AuthenticationHeaderValue("Basic", credString);
            
                //return new AuthenticationHeaderValue(
                //    "Basic",
                //    Convert.ToBase64String(
                //        System.Text.ASCIIEncoding.ASCII.GetBytes(
                //            string.Format(@"ServiceUser:{0}:{1}", Credientials.UserName, Credientials.Password))));


        }

        internal virtual string BuildFullURL(string relativeURL, string serviceName)
        {
            return @"http://" + _rootURL + @"/cobrawcfservices/" + serviceName + @"/" + relativeURL;

        }
        internal virtual string BuildFullURL(string relativeURL, string serviceName, Dictionary<string, string> data)
        {
            var baseURL = BuildFullURL(relativeURL, serviceName);
            if (data != null && data.Any())
            {
                baseURL = baseURL + "?" + data.First().Key + "=" + data.First().Value;
                for (var i = 1; i < data.Count(); i++)
                {
                    var value = data.ElementAt(i).Value;
                    if (String.IsNullOrEmpty(value))
                    {
                        baseURL = baseURL + "&" + data.ElementAt(i).Key;
                    }
                    else
                    {
                        baseURL = baseURL + "&" + data.ElementAt(i).Key + "=" + value;
                    }
                    
                }

            }
            return System.Uri.EscapeUriString(baseURL);

        }        
        internal async Task<string> AuthenticatedPostData(string relativeURL, string serviceName, Dictionary<string, string> data)
        {
            try
            {
                var url = BuildFullURL(relativeURL, serviceName, data);

                var client = GetClient();
                HttpResponseMessage response = await client.PostAsync(new Uri(url), new StringContent(""));
                //HttpResponseMessage response = await client.PostAsync(new Uri(url), new FormUrlEncodedContent(data));//would love to use content to post, but we use the QueryString

                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (Exception ex)
            {

            }
            return "";
        }

        private HttpClient GetClient()
        {
            // var authValue = GetAuthHeader();

            HttpClient client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = GetAuthHeader();//new AuthenticationHeaderValue("Basic", string.Format(@"ServiceUser:{0}:{1}", Credientials.UserName, Credientials.Password));
            var credString = string.Format(@"{0}:{1}:{2}", Credientials.UserName, Credientials.Password, Credientials.ImpersonateUserName);
            credString = credString.TrimEnd(':');
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", credString);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        internal async Task<string> AuthenticatedGetData(string relativeURL, string serviceName, Dictionary<string, string> data)
        {
            try
            {
                var client = GetClient();
                var url = BuildFullURL(relativeURL, serviceName, data);

                HttpResponseMessage response = await client.GetAsync(new Uri(url));

                response.Content.Headers.ContentType = new MediaTypeHeaderValue(@"application/json");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
            }
            catch (Exception ex)
            {

            }
            return "";
        }

        internal bool CanMakeServiceCall()
        {
            if (this.Credientials == null)
                return false;
            if (string.IsNullOrEmpty(_rootURL))
                return false;

            return !String.IsNullOrEmpty(Credientials.UserName) && !string.IsNullOrEmpty(Credientials.Password);
        }


        internal void SendStatus(string statusText) {

            var msg = new Messaging.StatusMessage(statusText, 0, false);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<Messaging.StatusMessage>(msg);
        }
        internal void SendStatus(string statusText, int percentComplete, bool isIndeterminate)
        {
            var msg = new Messaging.StatusMessage(statusText, percentComplete, isIndeterminate);
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<Messaging.StatusMessage>(msg);
        }
    }

}