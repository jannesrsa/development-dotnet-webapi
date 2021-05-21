using System.IO;
using System.Reflection;
using DevelopmentDotnetWebApi;
using DevelopmentDotnetWebApi.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace FunctionalTests
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class FunctionalTestDatabaseFixture
    {
        public const string SqliteConnectionStringKey = "SqliteConnectionString";
        private static readonly object _lock = new();
        private TestServer _testServer;

        public FunctionalTestDatabaseFixture()
        {
        }

        public TestServer CreateTestServer()
        {
            lock (_lock)
            {
                if (_testServer != null)
                {
                    return _testServer;
                }

                var path = Assembly.GetAssembly(typeof(FunctionalTestDatabaseFixture))
                   .Location;

                var hostBuilder = new WebHostBuilder()
                    .UseContentRoot(Path.GetDirectoryName(path))
                    .UseConfiguration(new ConfigurationHelper().Configuration)
                    .UseEnvironment("Development")
                    .UseStartup<Startup>();

                _testServer = new TestServer(hostBuilder);
                return _testServer;
            }
        }

        //private static IConfiguration GetConfiguration()
        //{
        //    var directory = Directory.GetCurrentDirectory();

        //    var builder = new ConfigurationBuilder()
        //       .SetBasePath(directory)
        //       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //       .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
        //       .AddEnvironmentVariables();

        //    //var dictionary = new Dictionary<string, string>
        //    //    {
        //    //       {$"ConnectionStrings:{SqliteConnectionStringKey}", $"Server=localhost,6433;Database={this.ReplacementDatabaseName};User Id=sa;Password=Pass@word;Persist Security Info=true;"}
        //    //    };

        //    //builder.AddInMemoryCollection(dictionary);

        //    return builder.Build();
        //}
    }
}