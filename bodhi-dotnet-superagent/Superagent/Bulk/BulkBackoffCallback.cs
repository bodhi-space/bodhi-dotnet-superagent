using Bodhi.Superagent.Backoff;
using Newtonsoft.Json.Linq;
using System.Net;
using unirest_net.http;

namespace Bodhi.Superagent
{
    public class BulkBackoffCallback : BackoffCallback<JToken>
    {

        private const string BULK_ID_HEADER = "bulk_id";

        public BulkBackoffCallback(BackoffConfig backoffConfig, BackoffResultHandler<JToken> backoffHandler):base(backoffConfig, backoffHandler)
        {
        }

        protected override Result<JToken> GetResult(HttpResponse<JToken> httpResponse)
        {
            Result<JToken> result;
            HttpStatusCode statusCode = Utils.GetStatusCode(httpResponse.Code);
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    result = new Result<JToken>(statusCode, null, httpResponse.Body);
                    break;
                case HttpStatusCode.Accepted:
                    string bulkId = httpResponse.Headers[BULK_ID_HEADER];
                    result = new Result<JToken>(statusCode, bulkId, null);
                    break;
                case HttpStatusCode.NoContent:
                    result = new Result<JToken>(statusCode, null, null);
                    break;
                default:
                    result = new Result<JToken>(statusCode, null, httpResponse.Body);
                    break;
            }
            return result;
        }
    }
}
