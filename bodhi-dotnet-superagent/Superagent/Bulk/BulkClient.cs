using Bodhi.Superagent.Backoff;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using unirest_net.http;
using unirest_net.request;

namespace Bodhi.Superagent
{

    public class BulkClient
    {

        private Credentials credentials;
        private ClientConfig clientConfig;

        public BulkClient(ClientConfig clientConfig)
        {
            this.clientConfig = clientConfig;
            this.credentials = clientConfig.Credentials;
        }

        public async Task<Result<JToken>> Get(string id)
        {
            return await BulkRequest(async () =>
            {
                string bulkUrl = clientConfig.GetNamespaceUri() + "/bulk/" + id;
                HttpRequest request = Unirest.get(bulkUrl);
                credentials.SetAuthentication(request);
                return await request.asJsonAsync<JToken>();
            });

        }

        public async Task<Result<JToken>> Post(Bulk bulk)
        {
            return await BulkRequest(async () =>
            {
                string bulkUrl = clientConfig.GetNamespaceUri() + "/bulk";
                HttpRequest request = Unirest.post(bulkUrl);
                credentials.SetAuthentication(request);
                return await request.body(bulk).asJsonAsync<JToken>();
            });

        }


        private async Task<Result<JToken>> BulkRequest(BackoffResultHandler<JToken> resultHandler)
        {
            BulkBackoffCallback backoffCallback = new BulkBackoffCallback(clientConfig.BackoffConfig, resultHandler);
            return await backoffCallback.complete();
        }

    }
}
