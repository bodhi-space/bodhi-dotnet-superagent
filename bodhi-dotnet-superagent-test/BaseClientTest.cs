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
            client = new Client("https://api.bodhi-dev.io", "jarsh", new BearerCredentials("eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJhWnRINE56ejBGZURjMU1FSkIyU2lIeHhGS0xuLXQtVm1DUHBHeUVnS09BIn0.eyJqdGkiOiI1NzI4MmM0YS0wMDE3LTQ1NWItOGM5NS02MWVlNzFkOWE1NDIiLCJleHAiOjE1NDM4NzIzNDYsIm5iZiI6MCwiaWF0IjoxNTQzODM2MzQ3LCJpc3MiOiJodHRwczovL2xvZ2luLmJvZGhpLWRldi5pby9hdXRoL3JlYWxtcy9ob3RzY2hlZHVsZXMiLCJhdWQiOiJ1cm46bWFjZTpvaWRjOmhvdHNjaGVkdWxlcy5jb20iLCJzdWIiOiJhZGZlNGQ1OS0xNzhhLTQyODctYjYzYS1kYWMxMDA0NDliZDIiLCJ0eXAiOiJJRCIsImF6cCI6InVybjptYWNlOm9pZGM6aG90c2NoZWR1bGVzLmNvbSIsImF1dGhfdGltZSI6MTU0MzgzNjM0Niwic2Vzc2lvbl9zdGF0ZSI6ImU3MWY3NDZlLWQyOTYtNDkwMi04MjVkLWNmNjk1ZjkyMmM3NiIsImFjciI6IjEiLCJoc3VzciI6ImFkbWluX19qYXJzaCIsIm90aGVyQ2xhaW1zIjp7ImJvZGhpYXBpIjoiYWRtaW5fX2phcnNoIn0sInByZWZlcnJlZF91c2VybmFtZSI6ImFkbWluX19qYXJzaCJ9.m1zbWqOd85OzKgaP21N16-mMmPRyTwgi9KipdHkusaBznlkT8AOfHOqM_Qbj_PLzVKwR_kxO58OsX734W3e9MDte7tp4g_7OlUUq0yW6u2yhxY21CEEYNT9r6hU05JlrZBESobLRD4IR7dzhPcPiwVpHd2XPBg8DtBFGxu2N7jZOGqFYqnzBWKGcxGPX4pGzWwMQZFh2OEhdd55Yx3qtm-075jveL7furO88nWNqUlzLx1D3SB8L9Rd0K4qIdiBO2p1HeF-QgWkoq7urjJyv7IVhsWKPURW7G3cemcqRbkn2_XtVl9Zjwbxm-REWerQqFxN4hWrKzV3TFN8BqYSQOA"));
        }
        [TestCleanup]
        public void Clean()
        {
            client = null;
        }

    }
}
