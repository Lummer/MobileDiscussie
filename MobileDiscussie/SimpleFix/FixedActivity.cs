using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Implementation;
using TheFactorM.Device;

namespace SimpleFix
{
    [Activity(Label = "SimpleFix", MainLauncher = true, Icon = "@drawable/icon")]
    public class FixedActivity : BaseActivity
    {
        private RequestViewModel _viewmodel;

        private Button _sendButton;
        private EditText _inputEditText;
        private TextView _outputTextView;

        #region Base class overrides

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _viewmodel = RequestViewModel.Instance;

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

            DiscussionController.LoadMainModel();

            _sendButton.Click += DoRequest;
            _viewmodel.PropertyChanged += ViewModelUpdate;

            DeviceContext.Current.Log.WriteInformational("OnStart");
        }

        /// <summary>
        /// Called when the activity is resuming
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();

            BindViewModel();
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
            _viewmodel.PropertyChanged -= ViewModelUpdate;
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

        private void DoRequest(object sender, EventArgs e)
        {
            int input = 0;
            if (int.TryParse(_inputEditText.Text, out input))
            {
                _viewmodel.DoRequest(input);
            }
            else
            {
                ShowErrorDialog("Input should be an integer!");
            }
        }

        private void ViewModelUpdate(object sender, System.ComponentModel.PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "ActionBusy" || args.PropertyName == "RequestStatus")
            {
                BindViewModel();
            }
            else if (args.PropertyName == "ErrorMessage")
            {
                ShowErrorDialog(_viewmodel.ErrorMessage);
            }
        }
        private void BindViewModel()
        {
            RunOnUiThread(() =>
            {
                try
                {
                    _sendButton.Enabled = !_viewmodel.ActionBusy;
                    _inputEditText.Enabled = !_viewmodel.ActionBusy;
                    _outputTextView.Text = _viewmodel.RequestStatus;
                }
                catch (Exception ex)
                {
                    DeviceContext.Current.Log.WriteError(ex);
                }
            });
        }
    }
}

