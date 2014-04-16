using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace MobileDiscussie.Controllers
{
    public class RequestController : ApiController
    {
        // GET api/values/5
        public string Get(int id)
        {
            Thread.Sleep(id);

            return string.Format(CultureInfo.InvariantCulture, "Waited for {0} milliseconds", id);
        }
    }
}