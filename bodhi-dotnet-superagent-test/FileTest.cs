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
   
        [TestMethod]
        public void TestUploadBody()
        {
            byte[] body = Encoding.UTF8.GetBytes("test body upload double");
            String uploadPath = "test-body-upload-dotnet/test-body-upload-dotnet.txt";
            DeleteFile(uploadPath);


            Task<Result<JToken>> resultTask = client.UploadFile(uploadPath, new ContentType("text/plain"), body);
            Result<JToken> result = resultTask.GetAwaiter().GetResult();
            TestFileResponseCreateValid(result);

        }

        [TestMethod]
        public void TestDoubleUploadBody()
        {
            byte[] body = Encoding.UTF8.GetBytes("test body upload double");
            String uploadPath = "test-body-upload-dotnet/test-body-upload-dotnet.txt";
            DeleteFile(uploadPath);

            Task<Result<JToken>> resultTask = client.UploadFile(uploadPath, new ContentType("text/plain"), body);
            Result<JToken> result = resultTask.GetAwaiter().GetResult();
            TestFileResponseCreateValid(result);
            resultTask = client.UploadFile(uploadPath, new ContentType("text/plain"), body);
            result = resultTask.GetAwaiter().GetResult();
            TestFileResponseUpdateValid(result);

        }


        [TestMethod]
        public void TestUploadFile()
        {
            using (TemporaryFile file = new TemporaryFile())
            {
                string uploadPath = "test-body-file-dotnet/test-body-file-dotnet.txt";
                DeleteFile(uploadPath);
                byte[] content = new byte[] { 3, 4, 5 };
                File.WriteAllBytes(file, content);

                Task<Result<JToken>> resultTask = client.UploadFile(uploadPath, new ContentType("application/octet-stream"), file);
                Result<JToken> result = resultTask.GetAwaiter().GetResult();
                TestFileResponseCreateValid(result);
            }

        }

        [TestMethod]
        public void TestDownloadFile() 
        {
            using (TemporaryFile file = new TemporaryFile())
            {

                string uploadPath = "test-download-file/test-download-file.txt";
                DeleteFile(uploadPath);
                byte[] content = new byte[] { 7, 8, 9 };
                File.WriteAllBytes(file, content);

                Task<Result<JToken>> resultTask = client.UploadFile(uploadPath, new ContentType("application/octet-stream"), file);
                Result<JToken> result = resultTask.GetAwaiter().GetResult();
                TestFileResponseCreateValid(result);

                Task<Result<Stream>> downloadResultTask = client.DownloadFile(uploadPath);
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

        [TestMethod]
        public void TestDownloadFileById() 
        {

            using (TemporaryFile file = new TemporaryFile())
            {
                string uploadPath = "test-download-file/test-download-file-by-id.txt";
                DeleteFile(uploadPath);
                byte[] content = new byte[] { 10, 11, 12 };
                File.WriteAllBytes(file, content);

                Task<Result<JToken>> resultTask = client.UploadFile(uploadPath, new ContentType("application/octet-stream"), file);
                Result<JToken> result = resultTask.GetAwaiter().GetResult();
                TestFileResponseCreateValid(result);

                string location = result.String;
                String id = location.Substring(location.LastIndexOf('/') + 1);

                Task<Result<Stream>> downloadResultTask = client.DownloadFile(id);
                Result<Stream> downloadResult = downloadResultTask.GetAwaiter().GetResult();

                Assert.AreEqual(HttpStatusCode.OK, downloadResult.StatusCode);

                Stream stream = downloadResult.Data;

                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    byte[] bytesRead = ms.ToArray();
                    Assert.AreEqual(content.Length, bytesRead.Length);
                    for (int i = 0; i < content.Length; i++)
                    {
                        Assert.AreEqual(content[i], bytesRead[i]);
                    }
                }
            }
        }

        private void DeleteFile(string deletePath)
        {
            Task<Result<JToken>> resultTask = client.DeleteFile(deletePath);
            Result<JToken> result = resultTask.GetAwaiter().GetResult();
            Assert.IsNotNull(result);
        }

        private void TestFileResponseCreateValid(Result<JToken> data)
        {
            Assert.AreEqual(HttpStatusCode.Created, data.StatusCode);
            Assert.IsNotNull(data.String);
        }

        private void TestFileResponseUpdateValid(Result<JToken> data)
        {
            Assert.AreEqual(HttpStatusCode.NoContent, data.StatusCode);
            Assert.IsNull(data.String);
        }


    }
}
