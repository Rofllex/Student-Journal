using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using KIRTStudentJournal.Controllers.API;
using Microsoft.IdentityModel.Protocols;
using KIRTStudentJournal.Infrastructure;

namespace KIRTStudentJournal
{
    public class Startup
    {
        /// <summary>
        /// ������� ����� web ������ �������
        /// </summary>
        public static event Action ApplicationStarted = () => { };
        /// <summary>
        /// ������� ����� web ������ ��������
        /// </summary>
        public static event Action ApplicationStopped = () => { };

        // ������ �� �������������
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        //public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = Jwt.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = Jwt.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = Jwt.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };
                });
            services.AddAuthorization(options => 
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();
            });
            services.AddControllers()
                .AddNewtonsoftJson();
            services.AddMvc(m => 
            {
                m.EnableEndpointRouting = false;
            }).AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.Use(async (context, next) =>  await new Infrastructure.CheckJwtMiddleware().Invoke(context, next));
            app.UseMvc(r =>
            {
                r.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            lifetime.ApplicationStarted.Register(() => ApplicationStarted());
            lifetime.ApplicationStopped.Register(() => ApplicationStopped());
        }
    }
}
