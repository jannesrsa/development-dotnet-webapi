using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using DevelopmentDotnetWebApi.Serialization;
using FunctionalTests.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace FunctionalTests.Tasks
{
    [Collection(FunctionalTestDatabaseCollection.CollectionName)]
    public class TasksScenarios : TasksScenariosBase
    {
        private readonly FunctionalTestDatabaseFixture _fixture;

        public TasksScenarios(FunctionalTestDatabaseFixture fixture, ITestOutputHelper output)
        {
            output.CreateSerilogLogger();

            _fixture = fixture;
        }

        [Fact]
        public async Task Get_Tasks()
        {
            // Action
            var response = await _fixture.CreateTestServer().CreateClient()
               .GetAsync(Get.Tasks());

            // Assert
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var actualServiceInstances = JsonSerializer.Deserialize<IEnumerable<DevelopmentDotnetWebApi.Models.Task>>(stringResponse, SerializationOptions.JsonSerializerOptions);

            Assert.NotNull(actualServiceInstances);
        }
    }
}