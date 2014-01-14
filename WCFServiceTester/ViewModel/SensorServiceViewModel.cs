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
        public SensorServiceViewModel():base("SensorService")
        {
            
        }
    }
}