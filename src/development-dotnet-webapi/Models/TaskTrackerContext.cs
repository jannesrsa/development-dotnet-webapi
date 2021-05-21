using DevelopmentDotnetWebApi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DevelopmentDotnetWebApi.Models
{
    public class TaskTrackerContext : DbContext
    {
        public TaskTrackerContext(DbContextOptions<TaskTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<Task> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var configurationHelper = new ConfigurationHelper();
            var sqliteDbConnectionString = configurationHelper.SqliteConnectionString;
            options.UseSqlite(sqliteDbConnectionString);
        }
    }

    public record Task(int Id, string Text, string Day, bool Reminder);
}