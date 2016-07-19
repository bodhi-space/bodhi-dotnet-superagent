using Bodhi.Superagent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Superagent.Test
{
    [TestClass]
    public class BaseClientTest
    {
        protected Client client;

        [TestInitialize]
        public void init()
        {
            client = new Client("https://api.bodhi-dev.io", "<namespace>", new BasicCredentials("<login>", "<password>"));
        }
        [TestCleanup]
        public void Clean()
        {
            client = null;
        }

    }
}
