using System;
using System.IO;
using System.Management.Automation;
using System.Threading;
using Xunit;

namespace Checkout.Tests.Acceptance.Helpers
{
    public class ContainerFixture : IDisposable
    {
        public ContainerFixture()
        {
            if (File.Exists("../../../../../docker-compose-acceptance.yml"))
            {
                using (var ps = PowerShell.Create())
                {
                    var asyncResult = ps
                        .AddScript(
                            @"dotnet publish ../../../../../src/Checkout.Web/Checkout.Web.csproj -o ..\..\..\..\..\obj\Docker\publish")
                        .AddScript("docker-compose -f ../../../../../docker-compose-acceptance.yml build")
                        .AddScript("docker-compose -f ../../../../../docker-compose-acceptance.yml up -d")
                        .BeginInvoke();

                    while (!asyncResult.IsCompleted)
                    {
                        Thread.Sleep(1000);
                    }

                    Thread.Sleep(1000);
                }
            }
        }

        public void Dispose()
        {
            if (File.Exists("../../../../../docker-compose-acceptance.yml"))
            {
                using (var ps = PowerShell.Create())
                {
                    var asyncResult = ps
                        .AddScript(
                            "docker rm $(docker stop $(docker ps -a -q --filter ancestor=checkout --format=\"{{.ID}}\"))")
                        .BeginInvoke();

                    while (!asyncResult.IsCompleted)
                    {
                        Thread.Sleep(1000);
                    }

                    Thread.Sleep(1000);
                }
            }
        }

        [CollectionDefinition("Container collection")]
        public class ContainerCollection : ICollectionFixture<ContainerFixture>
        {

        }
    }
}
