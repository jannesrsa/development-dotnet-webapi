using DevelopmentDotnetWebApi.Helpers;
using Serilog;
using Xunit.Abstractions;

namespace FunctionalTests.Extensions
{
    public static class TestOutputHelperExtensions
    {
        public static void CreateSerilogLogger(this ITestOutputHelper output)
        {
            var configurationHelper = new ConfigurationHelper();
            var loggerConfiguration = configurationHelper.ConfigureLogger();

            Log.Logger = loggerConfiguration
                .WriteTo.TestOutput(output)
                .CreateLogger();
        }
    }
}