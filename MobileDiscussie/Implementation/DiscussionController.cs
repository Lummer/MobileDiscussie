using Android.Util;
using Implementation.Manager;
using Polenter.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFactorM.Device;
using TheFactorM.Device.Persistence;

namespace Implementation
{
    /// <summary>
    /// Controller for the app
    /// Contains all business logic
    /// </summary>
    public class DiscussionController : IDisposable
    {
        #region Singleton

        private static DiscussionController _Instance = null;

        /// <summary>
        /// The controller instance to use.
        /// </summary>
        public static DiscussionController Instance
        {
            get { return _Instance ?? (_Instance = new DiscussionController()); }
        }

        #endregion

        private IRequestAgent _requestAgent;
        private static object FileLock = new object();

        private DiscussionController ()
        {
            _requestAgent = new RequestAgent();
        }

        /// <summary>
        /// Ensures that user is logged on or forced to
        /// </summary>
        /// <returns></returns>
        public async Task<string> DoRequest(int miliseconds)
        {
            string result = await _requestAgent.DoRequest(miliseconds);

            ApplyResponseToModel(result);

            await SaveMainModel();

            return result;
        }

        private void ApplyResponseToModel(string result)
        {
            MainModel.Instance.Result = string.Format(CultureInfo.InvariantCulture, "{0} -  {1}", DateTime.Now.ToLongTimeString(), result);
        }

        #region Business logic

        /// <summary>
        /// Loads the main model from local storage
        /// </summary>
        public static void LoadMainModel()
        {
            lock (FileLock)
            {
                try
                {
                    using (var storage = new IsolatedStorageUtil<MainModel>())
                    {
                        var savedModel = storage.LoadData(Constants.MainModelFileName, null);
                        MainModel.LoadInstance(savedModel);
                    }
                }
                catch (Exception e)
                {
                    DeviceContext.Current.Log.WriteError("Unable to load saved status from serialized MainModel, see exception details below");
                    DeviceContext.Current.Log.WriteError(e);
                }
            }
        }

        /// <summary>
        /// Saves the main model state to local storage
        /// </summary>
        /// <returns>The local dienst status.</returns>
        public static Task SaveMainModel()
        {
            var tcs = new TaskCompletionSource<bool>();
            try
            {
                using (var storage = new IsolatedStorageUtil<MainModel>())
                {
                    storage.SaveData(MainModel.Instance, Constants.MainModelFileName, null);
                    tcs.SetResult(true);
                }
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }

            return tcs.Task;
        }

        #endregion

        #region IDisposable members

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~DiscussionController()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing">disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                using (_requestAgent as IDisposable) { }
            }
        }

        #endregion
    }
}
