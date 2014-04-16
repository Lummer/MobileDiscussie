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
using System.Net.Http;
using System.Globalization;

namespace Implementation.Manager
{
    /// <summary>
    /// Implements IRequestAgent
    /// </summary>
    public class RequestAgent : IRequestAgent
    {
        private const string RequestUrl = "http://mobilediscussie.azurewebsites.net//api/request/{0}";

        /// <summary>
        /// Does  a request to the backend that takes the given length
        /// </summary>
        /// <param name="miliseconds">How long the request should take</param>
        /// <returns>Response string</returns>
        public async Task<string> DoRequest(int miliseconds)
        {
            string result = null;

            using (var client = new HttpClient())
            {
                string url = string.Format(CultureInfo.InvariantCulture, RequestUrl, miliseconds);
                var response = await client.GetAsync(url);
                result = await response.Content.ReadAsStringAsync();
            }

            return result;
        }
    }
}