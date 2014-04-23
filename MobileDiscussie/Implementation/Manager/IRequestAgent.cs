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
using System.Threading.Tasks;

namespace Implementation.Manager
{
    /// <summary>
    /// IRequestAgent
    /// </summary>
    public interface IRequestAgent
    {
        /// <summary>
        /// Does a request to the backend that takes the given length
        /// </summary>
        /// <param name="miliseconds">How long the request should take</param>
        /// <returns>Response string</returns>
        Task<string> DoRequest(int miliseconds);
    }
}