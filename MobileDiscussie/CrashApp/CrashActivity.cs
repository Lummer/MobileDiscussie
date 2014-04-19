using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Implementation;
using Android.Content.PM;
using System.Globalization;
using TheFactorM.Device;

namespace CrashApp
{
    [Activity(Label = "CrashApp", MainLauncher = true, Icon = "@drawable/icon")]
    public class CrashActivity : BaseActivity
    {
        private Button _sendButton;
        private EditText _inputEditText;
        private TextView _outputTextView;

        #region Base class overrides

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.RequestActivity);

            _sendButton = FindViewById<Button>(Resource.Id.send);
            _inputEditText = FindViewById<EditText>(Resource.Id.message);
            _outputTextView = FindViewById<TextView>(Resource.Id.result);

        }
        /// <summary>
        /// Called when the activity is started
        /// </summary>
        protected override void OnStart()
        {
            base.OnStart();

            DiscussieController.LoadMainModel();

            _sendButton.Click += DoRequest;
            DeviceContext.Current.Log.WriteInformational("OnStart");
        }

        /// <summary>
        /// Called when the activity is resuming
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();

            BindMainModel();
            DeviceContext.Current.Log.WriteInformational("OnResume");
        }

        /// <summary>
        /// Called when the activity is paused
        /// </summary>
        protected override void OnPause()
        {
            base.OnPause();
            DeviceContext.Current.Log.WriteInformational("OnPause");
        }

        /// <summary>
        /// Called when the activity is stopped
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();
            _sendButton.Click -= DoRequest;
            DeviceContext.Current.Log.WriteInformational("OnStop");
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            _sendButton.Dispose();
            _inputEditText.Dispose();
            _outputTextView.Dispose();

            DeviceContext.Current.Log.WriteInformational("OnDestroy");
        }

        #endregion

        private void BindMainModel()
        {
            RunOnUiThread(() =>
            {
                try
                {
                    _outputTextView.Text = MainModel.Instance.Result;
                }
                catch (Exception ex)
                {
                    DeviceContext.Current.Log.WriteError(ex);
                }
            });
        }

        private void DoRequest(object sender, EventArgs e)
        {
            Button typedSender = (Button)sender;

            typedSender.Enabled = false;
            _inputEditText.Enabled = false;

            _outputTextView.Text = "Request busy...";
            
            DeviceContext.Current.RunOnBackgroundThread(async () =>
            {
                try
                {
                    int input = 0;
                    if (int.TryParse(_inputEditText.Text, out input))
                    {
                        await DiscussieController.Instance.DoRequest(input);
                        BindMainModel();
                    }
                    else
                    {
                        ShowErrorDialog("Vul een integer in!");
                    }
                }
                catch (Exception ex)
                {
                    //do stuff with the error
                    DeviceContext.Current.Log.WriteError(ex);
                    ShowErrorDialog(ex.ToString());
                }
                finally
                {
                    RunOnUiThread(() =>
                    {
                        typedSender.Enabled = true;
                        _inputEditText.Enabled = true;
                    });
                }
            });
        }
    }
}

