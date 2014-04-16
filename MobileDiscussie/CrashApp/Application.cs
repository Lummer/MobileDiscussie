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
using TheFactorM.Device;

namespace CrashApp
{
    /// <summary>
    /// Represents the application.
    /// </summary>
    public class Application : global::Android.App.Application
    {
        /// <summary>
        /// Default constructor for this class.
        /// </summary>
        /// <param name="handle">Handle to the application class.</param>
        /// <param name="transfer"></param>
        public Application(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
        }

        /// <summary>
        /// Called when the application is being created.
        /// </summary>
        public override void OnCreate()
        {
            // Call base implementation
            base.OnCreate();

            // Initialize DeviceContext and set logging
            DeviceContext.Current.Log.ApplicationName = Resources.GetString(Resource.String.ApplicationName);
        }
    }
}