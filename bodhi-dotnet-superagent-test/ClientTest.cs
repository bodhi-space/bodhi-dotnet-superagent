
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bodhi.Superagent;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Net;

namespace Superagent.Test
{
    [TestClass]
    public class ClientTest : BaseClientTest
    {

        [TestMethod]
        public void TestGet()
        {
            Task<Result<JToken>> resultTask = client.Get("resources/Agent", null);
            Result<JToken> result = resultTask.GetAwaiter().GetResult();
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            JArray array = result.Data as JArray;
            Assert.IsNotNull(array);
            Assert.IsTrue(array.Count > 0);
        }

        [TestMethod]
        public void TestGetAll()
        {
            Task<Result<JToken>> resultTask = client.GetAll("resources/Agent", null);
            Result<JToken> result = resultTask.GetAwaiter().GetResult();
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            JArray array = result.Data as JArray;
            Assert.IsNotNull(array);
            Assert.IsTrue(array.Count > 0);

            resultTask = client.Get("resources/Agent/count", null);
            result = resultTask.GetAwaiter().GetResult();
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            JObject obj = result.Data as JObject;
            Assert.IsNotNull(obj);
            int count = (int)obj["count"];
            Assert.AreEqual(count, array.Count);

        }


        [TestMethod]
        public void TestPost()
        {
            JObject delete = new JObject();
            delete["role"] = "TEST-DOTNET-CLIENT-POST";

            Task<Result<JToken>> resultTask = client.Delete("resources/AgentAppRole", delete);
            Result<JToken> result = resultTask.GetAwaiter().GetResult();
            //Assert.AreEqual(204, result.StatusCode);

            JObject payload = new JObject();
            payload["app_id"] = "none";
            payload["role"] = "TEST-DOTNET-CLIENT-POST";

            String postUrl = "resources/AgentAppRole";

            resultTask = client.Post(postUrl, payload);
            result = resultTask.GetAwaiter().GetResult();
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            string location = result.String;
            Assert.IsNotNull(location);
            Assert.IsTrue(location.Contains(postUrl));


            resultTask = client.Get(location, null);
            result = resultTask.GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            JObject obj = result.Data as JObject;
            Assert.IsNotNull(obj);
            Assert.IsTrue(location.Contains((string)obj["sys_id"]));
        }

        [TestMethod]
        public void TestPut()
        {
            JObject delete = new JObject();
            delete["role"] = "TEST-DOTNET-CLIENT-POST";

            Task<Result<JToken>> resultTask = client.Delete("resources/AgentAppRole", delete);
            Result<JToken> result = resultTask.GetAwaiter().GetResult();
            //Assert.AreEqual(204, result.StatusCode);

            JObject payload = new JObject();
            payload["app_id"] = "none";
            payload["role"] = "TEST-DOTNET-CLIENT-POST";

            String postUrl = "resources/AgentAppRole";

            resultTask = client.Post(postUrl, payload);
            result = resultTask.GetAwaiter().GetResult();
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            string location = result.String;

            payload["app_id"] = "some-put";

            resultTask = client.Put(location, payload);
            result = resultTask.GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);


            resultTask = client.Get(location, null);
            result = resultTask.GetAwaiter().GetResult();
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            JObject obj = result.Data as JObject;
            Assert.IsNotNull(obj);
            Assert.IsTrue(location.Contains((string)obj["sys_id"]));
            Assert.AreEqual("some-put", (string)obj["app_id"]);
        }

        [TestMethod]
        public void TestPatch()
        {
            JObject delete = new JObject();
            delete["role"] = "TEST-DOTNET-CLIENT-POST";

            Task<Result<JToken>> resultTask = client.Delete("resources/AgentAppRole", delete);
            Result<JToken> result = resultTask.GetAwaiter().GetResult();
            //Assert.AreEqual(204, result.StatusCode);

            JObject payload = new JObject();
            payload["app_id"] = "none";
            payload["role"] = "TEST-DOTNET-CLIENT-POST";

            String postUrl = "resources/AgentAppRole";

            resultTask = client.Post(postUrl, payload);
            result = resultTask.GetAwaiter().GetResult();
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
            string location = result.String;

            Patch patch = new Patch(PatchOperation.REPLACE, "/app_id", "some-patch");
            resultTask = client.Patch(location, patch);
            result = resultTask.GetAwaiter().GetResult();
            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
            resultTask = client.Get(location, null);
            result = resultTask.GetAwaiter().GetResult();
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);

            JObject obj = result.Data as JObject;
            Assert.IsNotNull(obj);
            Assert.IsTrue(location.Contains((string)obj["sys_id"]));
            Assert.AreEqual("some-patch", (string)obj["app_id"]);
        }


    }
}
