﻿using Accounts.API.CorrelationId;
using Accounts.API.Features.Accounts.Aggregate;
using GlobalExceptionHandler.WebApi;
using Marten;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace Accounts.API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddSingleton<IDocumentStore>(c =>
                {
                    return DocumentStore.For(_ =>
                    {
                        _.Events.InlineProjections.AggregateStreamsWith<Account>(); //Recalculated projection on event capture thread
                        _.Connection("User ID=postgres;Password=changeme;Host=postgres_container;Port=5432;Database=Test");
                    });
                })
                .AddScoped(c => c.GetService<IDocumentStore>().LightweightSession())
                .AddScoped(c => c.GetService<IDocumentStore>().QuerySession())
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "CubeDash API", Version = "v1" });
                    c.CustomSchemaIds(x => x.FullName);
                })
                .AddMvc()
                .AddJsonOptions(o => o.SerializerSettings.Formatting = Formatting.Indented)
                .AddFeatureFolders()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCorrelationId();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseGlobalExceptionHandler(config =>
            {
                config.ContentType = "application/json";
                config.ResponseBody(s => 
                    JsonConvert.SerializeObject(new { s.Message }));
            });

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Accounts API V1");
            });
        }
    }
}
