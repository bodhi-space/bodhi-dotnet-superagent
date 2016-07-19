### Installation ###

1) Add a NuGet source using either this command line or Visual Studio interface
(Tools->Options->NuGet Package Manager->Package Sources)

        nuget sources Add -Name Artifactory -Source  https://rbcplatform.artifactoryonline.com/rbcplatform/api/nuget/bodhi-dotnet

2) Install bodhi-dotnet-superagent using either NuGet Package Manager Console (Tools->NuGet Package Manager-> Package Manager Console)

        Install-Package bodhi-dotnet-superagent


### Example ###

    using Bodhi.Superagent;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Bodhi.Superagent.Client
    {
        class Program
        {
            static void Main(string[] args)
            {
                Client client = new Client(Bodhi.Superagent.Environment.DEV, "<namespace>", new BasicCredentials("<login>", "<password>"));
                Task<Result<JToken>> resultTask = client.Get("resources/Agent", null);
                Result<JToken> result = resultTask.GetAwaiter().GetResult();
                Console.WriteLine("Status Code:"+result.StatusCode);
                Console.WriteLine("Found " + (result.Data as JArray).Count + " agents !");

            }
        }
    }

