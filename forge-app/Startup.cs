namespace forge_app
{
    using System;
    using forge_app.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        /* Startup class is responsible for configuring the server and its 'middleware'
         * We also try and retrieve the APS app client ID and secret client credential from environmental variables
         * To pass actual configuration we use appsettings.json and add APS_CLIENT_ID and APS_CLIENT_SECRET (add this file to .gitignore)
         */
        //constructor
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var clientID = Configuration["APS_CLIENT_ID"];
            var clientSecret = Configuration["APS_CLIENT_SECRET"];
            var bucket = Configuration["APS_BUCKET"]; // Optional
            if (string.IsNullOrEmpty(clientID) || string.IsNullOrEmpty(clientSecret))
            {
                throw new ApplicationException("Missing required environment variables APS_CLIENT_ID or APS_CLIENT_SECRET.");
            }
            services.AddSingleton<APS>(new APS(clientID, clientSecret, bucket));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
