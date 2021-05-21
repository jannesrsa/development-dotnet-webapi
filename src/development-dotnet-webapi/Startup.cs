using DevelopmentDotnetWebApi.Models;
using DevelopmentDotnetWebApi.Serialization;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;

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
            }

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevelopmentDotnetWebApi v1"));

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            // Gets a clean set of request logs containing summary data for each request
            // https://andrewlock.net/using-serilog-aspnetcore-in-asp-net-core-3-reducing-log-verbosity
            app.UseSerilogRequestLogging();

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
            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader());
            });

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    SerializationOptions.Configure(options.JsonSerializerOptions);
                });

            services.AddProblemDetails();

            services.AddApiVersioning(
                options =>
                {
                    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                });

            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });

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