using Bodhi.Superagent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Superagent.Test
{

    [TestClass]
    [Ignore]
    public class BulkTest: BaseClientTest
    {

        [TestMethod]
        public void TestBulk()
        {
            BulkConfig bulkConfig = new BulkConfig(ConfigOp.INSERT, true, "BulkTest");
            JArray payload = new JArray();

            JObject object1 = new JObject();
            object1.Add("name", "leo");
            object1.Add("other_field", "mauris non");
            payload.Add(object1);

            JObject object2 = new JObject();
            object2.Add("name", "orci");
            object2.Add("other_field", "sit amet justo");
            payload.Add(object2);

            JObject object3 = new JObject();
            object3.Add("name", "nulla");
            object3.Add("other_field", "pulvinar sed nisl nunc rhoncus");
            payload.Add(object3);

            Bulk bulk = new Bulk(bulkConfig, payload);

            Task<Result<JToken>> resultTask = client.PostBulk(bulk);
            Result<JToken> result = resultTask.GetAwaiter().GetResult();

            Assert.AreEqual(HttpStatusCode.Accepted, result.StatusCode);
            String bulk_id = result.String;
            Assert.IsNotNull(bulk_id);
            Assert.IsTrue(bulk_id.Length > 0);

            Thread.Sleep(5000);


            resultTask = client.GetBulk(bulk_id);
            result = resultTask.GetAwaiter().GetResult();
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            JArray array = result.Data as JArray;
            Assert.IsNotNull(array);
            Assert.AreEqual(3, array.Count);

        }
    }

}
