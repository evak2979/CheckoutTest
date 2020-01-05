using System;
using System.Diagnostics;
using System.IO;
using System.Management.Automation;
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
                    var results1 = ps
                        .AddScript(
                            @"dotnet publish ../../../../../src/Checkout.Web/Checkout.Web.csproj -o ..\..\..\..\..\obj\Docker\publish")
                        .AddScript("docker-compose -f ../../../../../docker-compose-acceptance.yml build")
                        .AddScript("docker-compose -f ../../../../../docker-compose-acceptance.yml up -d")
                        .Invoke();

                    foreach (var result in results1)
                    {
                        Debug.Write(result.ToString());
                    }
                }
            }
        }

        public void Dispose()
        {
            if (File.Exists("../../../../../docker-compose-acceptance.yml"))
            {
                using (var ps = PowerShell.Create())
                {
                    var results1 = ps
                        .AddScript(
                            "docker rm $(docker stop $(docker ps -a -q --filter ancestor=checkout --format=\"{{.ID}}\"))")
                        .Invoke();

                    foreach (var result in results1)
                    {
                        Debug.Write(result.ToString());
                    }
                }
            }
        }

        [CollectionDefinition("Container collection")]
        public class ContainerCollection : ICollectionFixture<ContainerFixture>
        {

        }
    }
}
