using Bodhi.Superagent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Superagent.Test { 
    [TestClass]
    public class NegativeTest: BaseClientTest
    {
        [TestMethod]
        public void TestGetNotJson()
        {
            Task<Result<JToken>> resultTask = client.Get("Something", null);
            Result<JToken> result = resultTask.GetAwaiter().GetResult();
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

    }

}
