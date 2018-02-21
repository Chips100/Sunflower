using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Autofac.Configuration;
using Sunflower.Business.Contracts;
using System.Runtime.Loader;
using System.Linq;

namespace Sunflower.App
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Read configurations.
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddAuthentication().AddCookie();
            services.AddMvc();
            services.AddCors();

            // Manual assembly scanning, as Autofac does not
            // respect DotNetCore environment.
            var assemblyFiles = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            var assemblies = assemblyFiles.Select(AssemblyLoadContext.Default.LoadFromAssemblyPath).ToArray();

            // Configure and register AutoFac for DI.
            var module = new ConfigurationModule(Configuration.GetSection("autoFac"));
            var builder = new ContainerBuilder();
            builder.RegisterModule(module);
            builder.Populate(services);
            return new AutofacServiceProvider(builder.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Configure CORS for the client application's origin.
            app.UseCors(c => c.WithOrigins(Configuration["ClientOrigin"])
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            // Create the persistent storage on Startup.
            var storageCreator = app.ApplicationServices.GetService<IStorageCreator>();
            storageCreator.EnsureCreated().Wait();
        }
    }
}