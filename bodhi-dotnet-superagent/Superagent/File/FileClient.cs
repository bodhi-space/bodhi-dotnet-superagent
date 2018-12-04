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
        private const string NAMESPACE_HEADER = "namespace";

        private ClientConfig clientConfig;
        private Credentials credentials;

        private const string FILES_SERVICE_PATH = "{0}/files/{2}/{1}";

        public FileClient(ClientConfig clientConfig)
        {
            this.clientConfig = clientConfig;
            this.credentials = clientConfig.Credentials;
        }

        public async Task<Result<JToken>> Upload(string uploadPath, ContentType contentType, string bucket, byte[] data)
        {
            return await FileRequest(async () =>
            {
                string fullUri = GetFilePath(uploadPath, bucket);
                string fileName = uploadPath.Substring(uploadPath.LastIndexOf('/') + 1);
                HttpRequest request = Unirest.put(fullUri);
                request.Headers.Add(NAMESPACE_HEADER, clientConfig.Namespace);
                credentials.SetAuthentication(request);
                request.field(UPLOAD_FILE_NAME, data, contentType, fileName);
                return await request.asJsonAsync<JToken>();
            });
        }

        public async Task<Result<JToken>> Upload(string uploadPath, ContentType contentType, string bucket, FileInfo file)
        {
            byte[] data = File.ReadAllBytes(@file.FullName);
            return await Upload(uploadPath, contentType, bucket, data);

        }

        public async Task<Result<Stream>> Download(string downloadPath, string bucket)
        {
            return await FileRequest(async () =>
            {
                string downloadUri = GetFilePath(downloadPath, bucket);
                HttpRequest request = Unirest.get(downloadUri);
                request.Headers.Add(NAMESPACE_HEADER, clientConfig.Namespace);
                credentials.SetAuthentication(request);
                return await request.asBinaryAsync();

            });
        }

        public async Task<Result<JToken>> Delete(string deletePath, string bucket)
        {
            return await FileRequest<JToken>(async () =>
            {
                string fullUri = GetFilePath(deletePath, bucket);
                HttpRequest request = Unirest.delete(fullUri);
                request.Headers.Add(NAMESPACE_HEADER, clientConfig.Namespace);
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


        private string GetFilePath(string path, string bucket)
        {
            return string.Format(FILES_SERVICE_PATH, clientConfig.GetFilesUri(), path, bucket);
        }

        private async Task<Result<T>> FileRequest<T> (BackoffResultHandler<T> resultHandler) where T : class
        {
            //have the same issue is for Java client.
            BackoffCallback<T> backoffCallback = new BackoffCallback<T>(clientConfig.BackoffConfig, resultHandler);
            return await backoffCallback.complete();
        }


    }
}
