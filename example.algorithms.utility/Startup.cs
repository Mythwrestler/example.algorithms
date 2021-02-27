using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace example.algorithms.utility
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string CORS_CONFIG_PROD = "cors_prod";
        readonly string CORS_CONFIG_DEV = "cors_dev";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "example.algorithms.utility", Version = "v1" });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: CORS_CONFIG_DEV,
                                builder =>
                                {
                                    builder
                                        .AllowAnyOrigin()
                                        .WithMethods("GET", "POST")
                                        .AllowAnyHeader();
                                }
                );

                options.AddPolicy(name: CORS_CONFIG_PROD,
                                builder =>
                                {
                                    builder
                                        .WithOrigins("https://projects.casperinc.net")
                                        .WithMethods("GET", "POST")
                                        .AllowAnyHeader();

                                }
                );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(CORS_CONFIG_DEV);
            }
            else
            {
                app.UseCors(CORS_CONFIG_PROD);
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "example.algorithms.utility v1"));

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
