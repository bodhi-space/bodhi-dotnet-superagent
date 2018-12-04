using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unirest_net.request;

namespace Bodhi.Superagent
{
    public class BearerCredentials : Credentials
    {
        private string token;
        public BearerCredentials(string token)
        {
            this.token = token;
        }

        public HttpRequest SetAuthentication(HttpRequest request)
        {
            return request.header("Authorization", "Bearer " + token);
        }
    }
}
