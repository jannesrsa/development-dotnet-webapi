using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace DevelopmentDotnetWebApi.Helpers
{
    public class ConfigurationHelper
    {
        public ConfigurationHelper()
        {
            this.Configuration = GetConfiguration();
        }

        public IConfiguration Configuration { get; }

        public LoggerConfiguration ConfigureLogger()
        {
            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.FromLogContext();

            loggerConfiguration.ReadFrom.Configuration(this.Configuration);
            return loggerConfiguration;
        }

        private static IConfiguration GetConfiguration()
        {
            var directory = Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder()
               .SetBasePath(directory)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
               .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}