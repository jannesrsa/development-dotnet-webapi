using Microsoft.EntityFrameworkCore;

namespace DevelopmentDotnetWebApi.Models
{
    public class TaskTrackerContext : DbContext
    {
        public TaskTrackerContext(DbContextOptions<TaskTrackerContext> options)
            : base(options)
        {
        }

        public const string ConnectionString = "Filename=tasks.db"; //"Filename=:memory:"

        public DbSet<Task> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(ConnectionString);
        }
    }

    public record Task(int Id, string Text, string Day, bool Reminder);
}