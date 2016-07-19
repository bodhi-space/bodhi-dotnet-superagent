using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bodhi.Superagent
{
    public class Utils
    {
        public static HttpStatusCode GetStatusCode(int code)
        {
            return (HttpStatusCode)HttpStatusCode.Parse(typeof(HttpStatusCode), code.ToString());
        }
    }
}
