using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Implementation
{
    /// <summary>
    /// MainModel
    /// </summary>
    [DataContract]
    public class MainModel : INotifyPropertyChanged
    {
        #region Singleton

        private static MainModel _Instance = null;

        /// <summary>
        /// The main model instance to use.
        /// </summary>
        [IgnoreDataMember]
        public static MainModel Instance
        {
            get
            {
                lock (_InstanceLock)
                {
                    return _Instance ?? (_Instance = new MainModel());
                }
            }
        }

        #endregion

        #region Privates & constructor

        private static object _InstanceLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="Implementation.MainModel"/> class.
        /// </summary>
        /// <remarks>Needed for (de)serialization</remarks>
        public MainModel() { }

        #endregion

        private string _result;

        /// <summary>
        /// Last result from the backend
        /// </summary>
        [DataMember]
        public string Result
        {
            get { return _result; }
            set
            {
                if (_result != value)
                {
                    _result = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Loads the instance from  an existing model instance
        /// </summary>
        /// <param name="model">Model.</param>
        public static void LoadInstance(MainModel model)
        {
            lock (_InstanceLock)
            {
                _Instance = model;
            }
        }

        #region INotifyPropertyChanged members

        /// <summary>
        /// Fires when a property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event
        /// </summary>
        /// <param name="propertyName"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "TheFactorM.Device.Logging.ApplicationLog.WriteInformational(System.String,System.Object[])", Justification = "MarcoK: Logging"),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "PropertyChanged", Justification = "MarcoK: Correct spelling"),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Is needed to support RaisePropertyChanged")]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}