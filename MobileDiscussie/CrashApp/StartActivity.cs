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

namespace CrashApp
{
    /// <summary>
    /// Start activity for launcher purposes
    /// </summary>
    [Activity(Label = "CrashApp", Icon = "@drawable/icon", Theme = "@style/Theme.Sherlock.Light", MainLauncher = true, NoHistory = true)]
    public class StartActivity : Activity
    {
        #region Base class overrides

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

        }

        protected override void OnResume()
        {
            base.OnResume();

            StartActivity(typeof(CrashActivity));
        }

        #endregion
    }
}