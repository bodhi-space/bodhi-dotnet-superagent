using Bodhi.Superagent.Backoff;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using unirest_net.http;
using unirest_net.request;

namespace Bodhi.Superagent
{

    public class FileClient
    {
        public const string UPLOAD_FILE_NAME = "uploadFile";
        private ClientConfig clientConfig;
        private Credentials credentials;

        public FileClient(ClientConfig clientConfig)
        {
            this.clientConfig = clientConfig;
            this.credentials = clientConfig.Credentials;
        }

        public async Task<Result<JToken>> Upload(string uploadPath, ContentType contentType, byte[] data)
        {
            return await FileRequest(async () =>
            {
                string fullUri = GetUploadUri(uploadPath);
                string fileName = uploadPath.Substring(uploadPath.LastIndexOf('/') + 1);
                HttpRequest request = Unirest.put(fullUri);
                credentials.SetAuthentication(request);
                request.field(UPLOAD_FILE_NAME, data, contentType, fileName);
                return await request.asJsonAsync<JToken>();
            });
        }

        public async Task<Result<JToken>> Upload(string uploadPath, ContentType contentType, FileInfo file)
        {
            byte[] data = File.ReadAllBytes(@file.FullName);
            return await Upload(uploadPath, contentType, data);

        }

        public async Task<Result<Stream>> Download(string downloadPath)
        {
            return await FileRequest(async () =>
            {
                string fullUri = clientConfig.GetNamespaceUri() + "/controllers/vertx/download/" + downloadPath;
                HttpRequest request = Unirest.get(fullUri);
                credentials.SetAuthentication(request);
                return await request.asBinaryAsync();

            });
        }

        public async Task<Result<JToken>> Delete(string deletePath)
        {
            return await FileRequest<JToken>(async () =>
            {
                string fullUri = GetUploadUri(deletePath);
                HttpRequest request = Unirest.delete(fullUri);
                credentials.SetAuthentication(request);
                //workaround for closed connection on delete
                try {
                    return await request.asJsonAsync<JToken>();
                } catch
                {
                    return await Task.Run(() =>
                    {
                        return new HttpResponse<JToken>(204, null);
                    });
                }

            });
        }


        private string GetUploadUri(string uploadPath)
        {
            return clientConfig.GetNamespaceUri() + "/controllers/vertx/upload/" + uploadPath;
        }

        private async Task<Result<T>> FileRequest<T> (BackoffResultHandler<T> resultHandler) where T : class
        {
            //have the same issue is for Java client.
            BackoffCallback<T> backoffCallback = new BackoffCallback<T>(clientConfig.BackoffConfig, resultHandler);
            return await backoffCallback.complete();
        }


    }
}
