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
            client = new Client(Bodhi.Superagent.Environment.DEV, "jarsh", new BearerCredentials("eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJhWnRINE56ejBGZURjMU1FSkIyU2lIeHhGS0xuLXQtVm1DUHBHeUVnS09BIn0.eyJqdGkiOiIyMjU5ZmEwMy1mYzExLTQyZTEtODNkNy1iYzMwY2NiMmE5N2UiLCJleHAiOjE1NDQxMjUxNjYsIm5iZiI6MCwiaWF0IjoxNTQ0MDg5MTY2LCJpc3MiOiJodHRwczovL2xvZ2luLmJvZGhpLWRldi5pby9hdXRoL3JlYWxtcy9ob3RzY2hlZHVsZXMiLCJhdWQiOiJ1cm46bWFjZTpvaWRjOmhvdHNjaGVkdWxlcy5jb20iLCJzdWIiOiJhZGZlNGQ1OS0xNzhhLTQyODctYjYzYS1kYWMxMDA0NDliZDIiLCJ0eXAiOiJJRCIsImF6cCI6InVybjptYWNlOm9pZGM6aG90c2NoZWR1bGVzLmNvbSIsImF1dGhfdGltZSI6MTU0NDA4OTE2Niwic2Vzc2lvbl9zdGF0ZSI6IjkxNzM1M2MxLWVhNTYtNDkwNy1hN2Q0LTRiNTFiMDcyODkyNyIsImFjciI6IjEiLCJoc3VzciI6ImFkbWluX19qYXJzaCIsIm90aGVyQ2xhaW1zIjp7ImJvZGhpYXBpIjoiYWRtaW5fX2phcnNoIn0sInByZWZlcnJlZF91c2VybmFtZSI6ImFkbWluX19qYXJzaCJ9.Y0epn8U_9ug4TEWDYRuGxaleCSVaCOeAyF_QB5C_6wNseCFviPnP_7JK4t8hDDGlquZY68rvQcFBONOzcThb8CWa0MlKuklIP6_mBtxEwSCP8mfwWG3eeZt5X23emWrhvg2FrQkwgN6ChUx_UP9IK75xad4yAxHqMObDE9NXMME311ZjqgVrPHKwU1A5gPSlXJEknetx8Omzi06TBrA0spIwmMotVNPI42r-wja1OprJAaYhsCDoMzVknSftuGQRWm6NtjVVMnCwPGafoYVoUiyln7FpsEddEK3GkVxreNEYr5xmFbLqT7WVNTK0fHKk6mRUXdtK6G7pjLcgD76fAw"));
        }
        [TestCleanup]
        public void Clean()
        {
            client = null;
        }

    }
}
