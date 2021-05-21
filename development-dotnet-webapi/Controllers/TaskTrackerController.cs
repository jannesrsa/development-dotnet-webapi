using System.Threading.Tasks;
using DevelopmentDotnetWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevelopmentDotnetWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskTrackerController : ControllerBase
    {
        private readonly TaskTrackerContext _taskTrackerContext;

        public TaskTrackerController(TaskTrackerContext taskTrackerContext)
        {
            _taskTrackerContext = taskTrackerContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tasks = await _taskTrackerContext.FindAsync<Models.Task>();
            return new JsonResult(tasks);
        }
    }
}
