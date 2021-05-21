using System;
using System.IO;
using DevelopmentDotnetWebApi.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DevelopmentDotnetWebApi
{
    public class Program
    {
        public static string Namespace = typeof(Startup).Namespace;
        public static string AppName = Namespace;

        public static void Main(string[] args)
        {
            var configurationHelper = new ConfigurationHelper();

            Log.Logger = CreateSerilogLogger(configurationHelper);

            try
            {
                Log.Information("Configuring web host ({ApplicationContext})...", Program.AppName);
                var host = BuildWebHost(configurationHelper, args);

                Log.Information("Starting web host ({ApplicationContext})...", Program.AppName);
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IWebHost BuildWebHost(ConfigurationHelper configurationHelper, string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(x => x.AddConfiguration(configurationHelper.Configuration))
                .UseStartup<Startup>()
                .UseSerilog(CreateSerilogLogger(configurationHelper))
                .Build();
        }

        private static ILogger CreateSerilogLogger(ConfigurationHelper configurationHelper)
        {
            return configurationHelper.ConfigureLogger()
                .CreateLogger();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}