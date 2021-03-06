﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using works.ei8.Identity.Data;
using works.ei8.Identity.Models;
using works.ei8.Identity.Services;
using static works.ei8.Identity.Constants;

namespace works.ei8.Identity
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Environment.GetEnvironmentVariable(EnvironmentVariableKeys.ConnectionStringsDefault))); 

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();

            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer(o => {
                    o.IssuerUri = Environment.GetEnvironmentVariable(EnvironmentVariableKeys.IssuerUri);
                    o.UserInteraction.LogoutUrl = Config.GetLogoutRedirectUri(Environment.GetEnvironmentVariable(EnvironmentVariableKeys.ClientsXamarin));
                })
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients(this.Configuration.GetSection("Clients")))
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<TestProfileService>();

            services.AddAntiforgery(o =>
            {
                o.SuppressXFrameOptionsHeader = true;
                // iframe
                // https://stackoverflow.com/questions/52669145/antiforgery-token-cookie-not-appearing-in-request-headers-only-in-when-embeded-i
                o.Cookie.SameSite = SameSiteMode.None;
            });
            // TODO: services.AddAuthentication()
            //    .AddGoogle("Google", options =>
            //    {
            //        // options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

            //        options.ClientId = "434483408261-55tc8n0cs4ff1fe21ea8df2o443v2iuc.apps.googleusercontent.com";
            //        options.ClientSecret = "3gcoTrEDPPJ0ukn_aYYT6PWo";
            //    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseMiddleware<HostNameMiddleware>();
            app.UseStaticFiles();

            // app.UseAuthentication();
            app.UseIdentityServer();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
