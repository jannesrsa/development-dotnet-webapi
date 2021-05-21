using DevelopmentDotnetWebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace DevelopmentDotnetWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TaskTrackerContext taskTrackerContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevelopmentDotnetWebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            taskTrackerContext.Database.Migrate();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<SqliteConnection>((sp) =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var sqliteDbConnectionString = configuration.GetConnectionString("SqliteConnectionString");
                return new SqliteConnection(sqliteDbConnectionString);
            });

            services.AddDbContext<TaskTrackerContext>((sp, options) =>
            {
                var sqliteConnection = sp.GetRequiredService<SqliteConnection>();
                options.UseSqlite(sqliteConnection);
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DevelopmentDotnetWebApi", Version = "v1" });
            });
        }
    }
}