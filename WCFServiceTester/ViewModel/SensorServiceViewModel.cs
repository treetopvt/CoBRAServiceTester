using GalaSoft.MvvmLight;

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
            : base("SensorService")
        {
        }

        /// <summary>
        /// The <see cref="WelcomeMessage" /> property's name.
        /// </summary>
        public const string WelcomeMessagePropertyName = "WelcomeMessage";

        private string _WelcomeMessage = "Welcome";

        /// <summary>
        /// Sets and gets the WelcomeMessage property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// This property's value is broadcasted by the MessengerInstance when it changes.
        /// </summary>
        public string WelcomeMessage
        {
            get
            {
                return _WelcomeMessage;
            }
            set
            {
                Set(WelcomeMessagePropertyName, ref _WelcomeMessage, value, true);
            }
        }

    }
}