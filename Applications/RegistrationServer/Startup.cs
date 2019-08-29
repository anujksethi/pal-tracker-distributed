﻿using Accounts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Projects;
using Steeltoe.CloudFoundry.Connector.MySql.EFCore;
using Users;
using Pivotal.Discovery.Client;
namespace RegistrationServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddDiscoveryClient(Configuration);
            services.AddDbContext<AccountContext>(options => options.UseMySql(Configuration));
            services.AddDbContext<ProjectContext>(options => options.UseMySql(Configuration));
            services.AddDbContext<UserContext>(options => options.UseMySql(Configuration));

            services.AddScoped<IAccountDataGateway, AccountDataGateway>();
            services.AddScoped<IProjectDataGateway, ProjectDataGateway>();
            services.AddScoped<IUserDataGateway, UserDataGateway>();
            services.AddScoped<IRegistrationService, RegistrationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
            app.UseDiscoveryClient();
        }
    }
}