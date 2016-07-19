using System.Net;
using System.Threading;
using System.Threading.Tasks;
using unirest_net.http;

namespace Bodhi.Superagent.Backoff
{
    public class BackoffCallback<T> where T : class
    {

        private const string LOCATION_HEADER = "Location";
        private BackoffResultHandler<T> backoffHandler;

        private BackoffConfig backoffConfig;
        private int retry = 1;

        public BackoffCallback(BackoffConfig backoffConfig, BackoffResultHandler<T> backoffHandler)
        {
            this.backoffConfig = backoffConfig;
            this.backoffHandler = backoffHandler;
        }

        private const int BACKOFF_STATUS_CODE = 429;

        public async Task<Result<T>> complete()
        {
            HttpResponse<T> httpResponse = await backoffHandler.Invoke();
            if (httpResponse.Code == BACKOFF_STATUS_CODE && retry <= backoffConfig.Retries)
            {
                Thread.Sleep(backoffConfig.WaitMillis);
                //log.info("Retrying in " + backoffConfig.getWaitMillis() + " milliseconds ..., retry " + retry + " of " + backoffConfig.getRetries());
                retry++;
                return await complete();
            }
            else {
                return GetResult(httpResponse);
            }
        }

        protected virtual Result<T> GetResult(HttpResponse<T> httpResponse)
        {
            Result<T> result;
            HttpStatusCode statusCode = Utils.GetStatusCode(httpResponse.Code);
            switch (statusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Accepted:
                    result = new Result<T>(statusCode, null, httpResponse.Body);
                    break;
                case HttpStatusCode.Created:
                    result = new Result<T>(statusCode, httpResponse.Headers[LOCATION_HEADER], null);
                    break;
                case HttpStatusCode.NoContent:
                    result = new Result<T>(statusCode, null, null);
                    break;
                default:
                    result = new Result<T>(statusCode, null, httpResponse.Body);
                    break;
            }
            return result;
        }

    }
}
