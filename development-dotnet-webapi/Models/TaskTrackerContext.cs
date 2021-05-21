using Microsoft.EntityFrameworkCore;

namespace DevelopmentDotnetWebApi.Models
{
    public class TaskTrackerContext : DbContext
    {
        public TaskTrackerContext(DbContextOptions<TaskTrackerContext> options)
            : base(options)
        {

        }

        private const string MemoryConnectionString = "Filename=:memory:"; //"Filename=:memory:"

        public DbSet<Task> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(MemoryConnectionString);
        }
    }

    public record Task(int Id, string Text, string Day, bool Reminder);
}