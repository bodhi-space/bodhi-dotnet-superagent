using Bodhi.Superagent.Backoff;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using unirest_net.http;
using unirest_net.request;

namespace Bodhi.Superagent
{

    public class Client
    {
        private const int DEFAULT_TIMEOUT = 30000;
        private const int DEFAULT_LIMIT = 100;

        private FileClient fileClientDelegate;
        private BulkClient bulkClientDelegate;
        private Credentials credentials;

        private ClientConfig clientConfig;

        public Client(string uri, string ns, Credentials credentials, long timeout)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            this.clientConfig = new ClientConfig(uri, ns, credentials, new BackoffConfig());
            this.credentials = clientConfig.Credentials;
            this.fileClientDelegate = new FileClient(clientConfig);
            this.bulkClientDelegate = new BulkClient(clientConfig);
            //Unirest.setTimeouts(timeout, timeout);
        }

        public Client(string uri, string ns, Credentials credentials) : this(uri, ns, credentials, DEFAULT_TIMEOUT)
        {

        }

        public Client(Environment env, string ns, Credentials credentials) : this(env.getUrl(), ns, credentials, DEFAULT_TIMEOUT)
        {

        }

        public Client(Environment env, string ns, Credentials credentials, long timeout) : this(env.getUrl(), ns, credentials, timeout)
        {

        }



        //GET METHODS
        //
        public async Task<Result<JToken>> Get(string url, JObject query)
        {
            return await Request(async () =>
            {
                string fullUrl = GetUrl(url, query);
                HttpRequest request = Unirest.get(fullUrl);
                request.header("accept", "application/json");
                credentials.SetAuthentication(request);
                return await request.asStringAsync();
            });
        }


        public async Task<Result<JToken>> GetAll(string url, JObject query)
        {
            return await GetPages(new JArray(), url, query, 1);
        }

        public async Task<Result<JToken>> GetPage(string url, JObject query, int page, int limit)
        {
            string pagingParam = "?paging=limit:" + limit + ",page:" + page;
            return await Get(url + pagingParam, query);

        }
        //POST METHODS
        public async Task<Result<JToken>> Post(string url, JToken body)
        {
            return await Request(async () =>
            {
                HttpRequest request = CreatePayloadRequest(HttpMethod.Post, url);
                string payload = body.ToString();
                return await request.body(payload).asStringAsync();
            });
        }

        //PUT METHOD
        public async Task<Result<JToken>> Put(string url, JToken body)
        {
            return await Request(async () =>
            {
                HttpRequest request = CreatePayloadRequest(HttpMethod.Put, url);
                string payload = body.ToString();
                return await request.body(payload).asStringAsync();
            });
        }

        //PATCH METHOD
        public async Task<Result<JToken>> Patch(string url, params Patch[] patches)
        {
            return await Request(async () =>
            {
                HttpRequest request = CreatePayloadRequest(new HttpMethod("PATCH"), url);
                JArray patchArray = new JArray();
                foreach (Patch patch in patches)
                {
                    patchArray.Add(patch);
                }
                string payload = patchArray.ToString();
                return await request.body(payload).asStringAsync();
            });
        }


        //DELETE METHODS
        public async Task<Result<JToken>> Delete(String url, JObject query)
        {
            return await Request(async () =>
            {
                String fullUrl = GetUrl(url, query);
                HttpRequest request = Unirest.delete(fullUrl);
                credentials.SetAuthentication(request);
                return await request.asStringAsync();
            });

        }


        //FILE METHODS
        public async Task<Result<JToken>> UploadFile(string uploadPath, ContentType contentType, string bucket, byte[] body) {
            return await fileClientDelegate.Upload(uploadPath, contentType, bucket, body);
        }

        public async Task<Result<JToken>> UploadFile(string uploadPath, ContentType contentType, String bucket, FileInfo file) {
            return await fileClientDelegate.Upload(uploadPath, contentType, bucket, file);
        }

        public async Task<Result<Stream>> DownloadFile(string downloadPath, string bucket) {
            return await fileClientDelegate.Download(downloadPath, bucket);
        }

        public async Task<Result<JToken>> DeleteFile(string deletePath, string bucket) {
            return await fileClientDelegate.Delete(deletePath, bucket);
        }

        //BULK METHODS
        public async Task<Result<JToken>> PostBulk(Bulk bulk) {
            return await bulkClientDelegate.Post(bulk);
        }

        public async Task<Result<JToken>> GetBulk(string id) {
            return await bulkClientDelegate.Get(id);
        }

        // PRIVATE METHODS

        private async Task<Result<JToken>> Request(BackoffResultHandler<string> resultHandler)
        {
            //have the same issue is for Java client.
            BackoffCallback<string> backoffCallback = new BackoffCallback<string>(clientConfig.BackoffConfig, resultHandler);
            Result<string> result = await backoffCallback.complete();
            try
            {
                JToken data = result.Data != null ? JToken.Parse(result.Data) : null;
                return new Result<JToken>(result.StatusCode, result.String, data);
            }
            catch (Exception ex)
            {
                return new Result<JToken>(HttpStatusCode.InternalServerError, ex.Message, null);

            }
        }

        private HttpRequest CreatePayloadRequest(HttpMethod method, string url)
        {
            string fullUrl = GetUrl(url, null);
            HttpRequest request = new HttpRequest(method, fullUrl);
            credentials.SetAuthentication(request);
            return request;
        }



        private string GetUrl(string url, JObject query)
        {
            StringBuilder result = new StringBuilder();
            if (url.StartsWith("/"))
            {
                result.Append(clientConfig.Uri).Append(url);
            }
            else {
                result.Append(clientConfig.GetNamespaceUri()).Append('/').Append(url);
            }
            if (query != null)
            {
                result.Append(result.ToString().Contains("paging=") ? "&where=" : "?where=");
                result.Append(WebUtility.UrlEncode(query.ToString()));
            }
            return result.ToString();
        }

        private async Task<Result<JToken>> GetPages(JArray result, string url, JObject query, int page)
        {

            Result<JToken> pageResult = await GetPage(url, query, page, DEFAULT_LIMIT);
            if (pageResult.StatusCode == HttpStatusCode.OK)
            {
                JArray array = (JArray)pageResult.Data;
                if (array != null && array.Count > 0)
                {
                    foreach (JToken token in array)
                    {
                        result.Add(token);
                    }
                    int nextPage = page + 1;

                    return await GetPages(result, url, query, nextPage);
                }
                else {
                    return new Result<JToken>(HttpStatusCode.OK, null, result);
                }
            }
            else {
                return pageResult;
            }
        }

    }
}

