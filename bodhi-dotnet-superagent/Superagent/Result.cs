using System.Net;

namespace Bodhi.Superagent
{

    public class Result<T>
    {
        private HttpStatusCode statusCode;
        private string strValue;
        private T data;

        public Result(HttpStatusCode statusCode, string strValue, T data)
        {
            this.statusCode = statusCode;
            this.strValue = strValue;
            this.data = data;
        }

        public HttpStatusCode StatusCode
        {
            get
            {
                return statusCode;
            } 
        }

        public string String
        {
            get
            {
                return strValue;
            }
        }

        public T Data
        {
            get
            {
                return data;
            }
        }
    }
}
