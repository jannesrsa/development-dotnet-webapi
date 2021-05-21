namespace FunctionalTests.Tasks
{
    public class TasksScenariosBase
    {
        private const string ApiUrlBase = "api/v1";

        public static class Get
        {
            private static readonly string ApiTasks = $"{ApiUrlBase}/tasks";

            public static string Tasks()
            {
                return $"{ ApiTasks }";
            }
        }
    }
}