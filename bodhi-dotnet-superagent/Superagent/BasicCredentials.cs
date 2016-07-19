using System;
using System.Text;
using unirest_net.request;

namespace Bodhi.Superagent
{
    public class BasicCredentials : Credentials
    {

        private string username;
        private string password;

        public BasicCredentials(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        public HttpRequest SetAuthentication(HttpRequest request)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(username+":"+password);
            string auth = Convert.ToBase64String(plainTextBytes);
            return request.header("Authorization","Basic "+auth);
        }
    }
}
