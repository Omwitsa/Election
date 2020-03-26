using elections.IRepository;
using elections.IServices;
using elections.Models;
using elections.Repository;
using elections.Services;
using elections.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace elections
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
			var issuer = Configuration["AuthSettings:Issuer"];
			var audience = Configuration["AuthSettings:Audience"];
			var sharedKey = Configuration["AuthSettings:SigningKey"];
			var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sharedKey));
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					// Clock skew compensates for server time drift.
					// We recommend 5 minutes or less:
					ClockSkew = TimeSpan.FromMinutes(5),
					// Specify the key used to sign the token:
					IssuerSigningKey = signingKey,
					RequireSignedTokens = true,
					ValidateIssuerSigningKey = true,
					// Ensure the token hasn't expired:
					RequireExpirationTime = true,
					ValidateLifetime = true,
					// Ensure the token audience matches our audience value (default true):
					ValidateAudience = true,
					ValidAudience = audience,
					// Ensure the token was issued by a trusted authorization server (default true):
					ValidateIssuer = true,
					ValidIssuer = issuer
				};
			});

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
			
			var electionMembership = DbSetting.ConnectionString(Configuration, "Portal");
			services.AddDbContext<ElectionsDbContest>(options => options.UseSqlServer(electionMembership));
			services.AddSingleton<IUnisolApiProxy, UnisolApiProxy>();
			services.AddTransient<IUnitOfWork, UnitOfWork>();
			services.AddTransient<IUserServices, UserServices>();
			services.AddTransient<IAuthService, AuthService>();
			services.AddTransient<IEmailService, EmailService>();
		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
			//GlobalDiagnosticsContext.Set("connectionString", Configuration.GetConnectionString("Campus360Entities"));

			if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

			app.UseAuthentication();
			app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

			app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
			
			UpdateDatabase(app);
		}

		//this method runs migrations automatically
		private static void UpdateDatabase(IApplicationBuilder app)
		{
			using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
			{
				var context = serviceScope.ServiceProvider.GetRequiredService<ElectionsDbContest>();
				context.Database.Migrate();
				context.EnsureDatabaseSeeded();
				// context.Database.EnsureCreated();
			}
		}
	}
}
