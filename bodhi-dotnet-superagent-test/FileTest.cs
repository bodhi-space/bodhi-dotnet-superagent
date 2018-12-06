using Bodhi.Superagent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Superagent.Test
{
    [TestClass]
    public class FileTest: BaseClientTest
    {

        private const string DEFAULT_CONTENT_TYPE = "text/plain";
        private const string DEFAULT_BINARY_CONTENT_TYPE = "application/octet-stream";

        private const string DEFAULT_BUCKET = "apps";

        [TestMethod]
        public void TestUploadBody()
        {
            byte[] body = Encoding.UTF8.GetBytes("test body upload double");
            String uploadPath = "test-body-upload-dotnet/test-body-upload-dotnet.txt";
            DeleteFile(uploadPath, DEFAULT_BUCKET);


            Task<Result<JToken>> resultTask = client.UploadFile(uploadPath, new ContentType(DEFAULT_CONTENT_TYPE), DEFAULT_BUCKET, body);
            Result<JToken> result = resultTask.GetAwaiter().GetResult();
            TestFileResponseUploadValid(result);

        }

        [TestMethod]
        public void TestDoubleUploadBody()
        {
            byte[] body = Encoding.UTF8.GetBytes("test body upload double");
            String uploadPath = "test-body-upload-dotnet/test-body-upload-dotnet.txt";
            DeleteFile(uploadPath, DEFAULT_BUCKET);

            Task<Result<JToken>> resultTask = client.UploadFile(uploadPath, new ContentType(DEFAULT_CONTENT_TYPE), DEFAULT_BUCKET, body);
            Result<JToken> result = resultTask.GetAwaiter().GetResult();
            TestFileResponseUploadValid(result);
            resultTask = client.UploadFile(uploadPath, new ContentType(DEFAULT_CONTENT_TYPE), DEFAULT_BUCKET, body);
            result = resultTask.GetAwaiter().GetResult();
            TestFileResponseUploadValid(result);

        }


        [TestMethod]
        public void TestUploadFile()
        {
            using (TemporaryFile file = new TemporaryFile())
            {
                string uploadPath = "test-body-file-dotnet/test-body-file-dotnet.txt";
                DeleteFile(uploadPath, DEFAULT_BUCKET);
                byte[] content = new byte[] { 3, 4, 5 };
                File.WriteAllBytes(file, content);

                Task<Result<JToken>> resultTask = client.UploadFile(uploadPath, new ContentType(DEFAULT_BINARY_CONTENT_TYPE), DEFAULT_BUCKET, file);
                Result<JToken> result = resultTask.GetAwaiter().GetResult();
                TestFileResponseUploadValid(result);
            }

        }

        [TestMethod]
        public void TestDownloadFile() 
        {
            using (TemporaryFile file = new TemporaryFile())
            {

                string uploadPath = "test-download-file/test-download-file.txt";
                DeleteFile(uploadPath, DEFAULT_BUCKET);
                byte[] content = new byte[] { 7, 8, 9 };
                File.WriteAllBytes(file, content);

                Task<Result<JToken>> resultTask = client.UploadFile(uploadPath, new ContentType(DEFAULT_BINARY_CONTENT_TYPE), DEFAULT_BUCKET, file);
                Result<JToken> result = resultTask.GetAwaiter().GetResult();
                TestFileResponseUploadValid(result);

                Task<Result<Stream>> downloadResultTask = client.DownloadFile(uploadPath, DEFAULT_BUCKET);
                Result<Stream> downloadResult = downloadResultTask.GetAwaiter().GetResult();
        
                Assert.AreEqual(HttpStatusCode.OK, downloadResult.StatusCode);

                Stream stream = downloadResult.Data;

                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    byte[] bytesRead = ms.ToArray();
                    Assert.AreEqual(content.Length, bytesRead.Length);
                    for  (int i =0; i<content.Length; i++)
                    {
                        Assert.AreEqual(content[i], bytesRead[i]);
                    }
                }
            }

        }

        private void DeleteFile(string deletePath, string bucket)
        {
            Task<Result<JToken>> resultTask = client.DeleteFile(deletePath, bucket);
            Result<JToken> result = resultTask.GetAwaiter().GetResult();
            Assert.IsNotNull(result);
        }

        private void TestFileResponseUploadValid(Result<JToken> data)
        {
            Assert.AreEqual(HttpStatusCode.Accepted, data.StatusCode);
            Assert.IsNotNull(data.Data);
            Assert.AreEqual(1, data.Data.Children().Count<JToken>());
            var token = data.Data.Children().First();
            Assert.IsTrue(token is JObject);
            Assert.AreEqual(DEFAULT_BUCKET, token["bucket"]);
        }

    }
}
