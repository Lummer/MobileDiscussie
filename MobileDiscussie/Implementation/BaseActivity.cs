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

namespace Implementation
{
    /// <summary>
    /// BaseActivity contains some clutter that I don't want to show in the demo
    /// </summary>
    public abstract class BaseActivity : Activity, IDialogInterfaceOnClickListener
    {
        protected static AlertDialog _dialog;

        protected void ShowErrorDialog(string message)
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
    }
}