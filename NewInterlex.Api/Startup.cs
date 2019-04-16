using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NewInterlex.Api.Models.Settings;
using NewInterlex.Infrastructure.Auth;
using NewInterlex.Infrastructure.Data;
using NewInterlex.Infrastructure.Helpers;
using NewInterlex.Infrastructure.Identity;
using Serilog.Extensions;

namespace NewInterlex.Api
{
    using Core;
    using Infrastructure;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(this.Configuration.GetConnectionString("Default")));
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(this.Configuration.GetConnectionString("Default")));
            var authSettings = Configuration.GetSection(nameof(AuthSettings));
            services.Configure<AuthSettings>(authSettings);
            var signingKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authSettings[nameof(AuthSettings.SecretKey)]));

            var jwtOptions = Configuration.GetSection(nameof(JwtOptions));
            services.Configure<JwtOptions>(options =>
            {
                options.Audience = jwtOptions[nameof(JwtOptions.Audience)];
                options.Issuer = jwtOptions[nameof(JwtOptions.Issuer)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            ConfigureAuthentication(services, jwtOptions, signingKey);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("User",
                    policy => { policy.RequireClaim(Constants.JwtClaimIdentifiers.Rol, Constants.JwtClaims.User); });

                // maybe use policy here, also add Admin, Editor whatever
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
            services.AddAutoMapper();

            //Autofac
            var builder = new ContainerBuilder();
            builder.RegisterModule(new CoreModule());
            builder.RegisterModule(new InfrastructureModule());
            // register modules here
            builder.Populate(services);
            var container = builder.Build(); //this could be a property on the class if needed
            return new AutofacServiceProvider(container);
        }

        private void ConfigureAuthentication(IServiceCollection services, IConfigurationSection jwtOptions,
            SymmetricSecurityKey signingKey)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtOptions[nameof(JwtOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtOptions[nameof(JwtOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false, // maybe true??
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = tokenValidationParameters;
                options.ClaimsIssuer = jwtOptions[nameof(JwtOptions.Issuer)];
                options.SaveToken = true;

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseExceptionHandler(
            //    builder =>
            //    {
            //        builder.Run(
            //            async context =>
            //            {
            //                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            //                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

            //                var error = context.Features.Get<IExceptionHandlerFeature>();
            //                if (error != null)
            //                {
            //                    context.Response.AddApplicationError(error.Error.Message);
            //                    await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
            //                }
            //            });
            //    });

            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}