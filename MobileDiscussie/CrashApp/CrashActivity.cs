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
using Xamarin.ActionbarSherlockBinding.App;
using TheFactorM.Device;

namespace CrashApp
{
    [Activity(Label = "CrashApp", Icon = "@drawable/icon", Theme = "@style/Theme.Sherlock.Light", ConfigurationChanges = ConfigChanges.Orientation)]
    public class CrashActivity : SherlockActivity, IDialogInterfaceOnClickListener
    {
        private static AlertDialog _dialog;

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

        private void ShowErrorDialog(string message)
        {
            RunOnUiThread(() =>
            {
                var builder = new AlertDialog.Builder(this)
                                .SetTitle("Error")
                                .SetMessage(message)
                                .SetPositiveButton(Resource.String.Ok, this)
                                .SetCancelable(false);

                if (_dialog != null && _dialog.IsShowing)
                {
                    _dialog.Dismiss();
                }
                _dialog = builder.Create();
                _dialog.Show();
            });
        }

        #region IDialogInterfaceOnClickListener

        /// <summary>
        /// This method will be invoked when a button in the dialog is clicked. 
        /// </summary>
        /// <param name="dialog">The dialog that received the click.</param>
        /// <param name="which">The button that was clicked</param>
        public void OnClick(IDialogInterface dialog, int which)
        {
            dialog.Dismiss();
        }

        #endregion

        /// <summary>
        /// Called when the activity has detected the user's press of the back key.
        /// </summary>
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            this.Finish();
        }
    }
}

