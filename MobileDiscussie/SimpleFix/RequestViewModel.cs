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
using Implementation;
using System.Runtime.CompilerServices;
using TheFactorM.Device;

namespace SimpleFix
{
    /// <summary>
    /// ViewModel that fixes the crash
    /// </summary>
    public class RequestViewModel : INotifyPropertyChanged
    {
        #region privates

        private bool _actionBusy;
        private string _requestStatus;
        private string _errorMessage;
        private DiscussionController _discussieController;

        #endregion

        #region Properties
        
        /// <summary>
        /// If the is being refreshed
        /// </summary>
        public bool ActionBusy
        {
            get { return _actionBusy; }
            set
            {
                if (_actionBusy != value)
                {
                    _actionBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Dienst
        /// </summary>
        public string RequestStatus
        {
            get { return _requestStatus; }
            set
            {
                if (_requestStatus != value)
                {
                    _requestStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ErrorMessage 
        {
            get { return _errorMessage; }
            set
            {
                if (_errorMessage != value)
                {
                    _requestStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion
        
        #region Actions

        /// <summary>
        /// Refreshes the status of the dienst
        /// </summary>
        /// <param name="lenght"></param>
        public async void DoRequest(int lenght)
        {
            if (!ActionBusy)
            {
                ActionBusy = true;
                try
                {
                    RequestStatus = "Request busy...";

                    await DiscussionController.Instance.DoRequest(lenght);
                    
                    RequestStatus = MainModel.Instance.Result;
                }
                catch (Exception e)
                {
                    ErrorMessage = e.ToString();
                }
                finally
                {
                    ActionBusy = false;
                }
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates the view model
        /// </summary>
        private RequestViewModel()
        {
            _discussieController = DiscussionController.Instance;
        }

        #endregion

        #region Singleton

        private static RequestViewModel _instance = null;
        private static object _instanceLock = new object();

        /// <summary>
        /// The main model instance to use.
        /// </summary>
        public static RequestViewModel Instance
        {
            get 
            { 
                lock (_instanceLock)
                {
                    return _instance ?? (_instance = new RequestViewModel()); 
                }
            } 
        }

        #endregion

        #region INotifyPropertyChanged

        /// <summary>
        /// Fires when a property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event
        /// </summary>
        /// <param name="propertyName"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Justification = "Is needed to support RaisePropertyChanged")]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
            else
            {
                DeviceContext.Current.Log.WriteInformational("PropertyChanged event is null. No trigger performed for: {0}", propertyName);
            }
        }

        #endregion
    }
}