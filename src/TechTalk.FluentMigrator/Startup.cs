using FluentMigrator.Domain.Entities;
using FluentMigrator.Infra.Middlewares;
using FluentMigrator.Infra.Migrations;
using Marten;
using Marten.Schema;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace FluentMigrator.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FluentMigrator", Version = "v1" });
            });

            services.AddMarten(opts =>
            {
                opts.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
                opts.Connection(Configuration.GetConnectionString("DatabaseConnection"));

                opts.Schema.For<WeatherForecast>()
                        .UniqueIndex(UniqueIndexType.DuplicatedField, x => x.Id);

                //opts.InitialData.Add(new InitialData());
            });

            services.AddFluentMigrator(Configuration.GetConnectionString("DatabaseConnection"),
                typeof(WeatherForecast_AddDescriptionProperty).Assembly);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FluentMigrator v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
