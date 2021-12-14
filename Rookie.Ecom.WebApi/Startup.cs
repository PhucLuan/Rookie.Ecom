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
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rookie.Ecom.Business;
using Rookie.Ecom.Business.Security.Requirement;
using Rookie.Ecom.IdentityServer;
using Rookie.Ecom.IdentityServer.Data;
using Rookie.Ecom.IdentityServer.Models;
using Rookie.Ecom.WebApi.Filters;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rookie.Ecom.WebApi
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
            //Enable CORS (Cross Orign Resource Sharing)//http://localhost:3000 
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options =>
                options.WithOrigins(
                    "http://localhost:3000").WithHeaders(" * ").WithMethods("*"));
            });


            services.AddBusinessLayer(Configuration);

            services
                .AddControllers(x =>
                    {
                        x.Filters.Add(typeof(ValidatorActionFilter));
                    })
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                })
                .AddJsonOptions(ops =>
                {
                    ops.JsonSerializerOptions.IgnoreNullValues = true;
                    ops.JsonSerializerOptions.WriteIndented = true;
                    ops.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    ops.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    ops.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                    );

            //Add Authen Scheme
            // services.AddAuthentication("Bearer")
            //     .AddIdentityServerAuthentication("Bearer", options =>
            //{
            //    // required audience of access tokens
            //    options.ApiName = "api1";

            //    // auth server base endpoint (this will be used to search for disco doc)
            //    options.Authority = "https://localhost:5000";


            //});

            // accepts any access token issued by identity server
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5000";

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            //Add Swagger Authen
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Protected API", Version = "v1" });

                // we're going to be adding more here...
                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://localhost:5000/connect/authorize"),
                            TokenUrl = new Uri("https://localhost:5000/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {"api1", "Demo API - full access"}
                            }
                        }
                    }
                });


                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ADMIN_ROLE_POLICY", policy =>
                {
                    //policy.AddAuthenticationSchemes("Bearer");
                    policy.RequireAuthenticatedUser();
                    policy.Requirements.Add(new AdminRoleRequirement());
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Rookie.Ecom.WebApi ProtectApi V1");

                    options.OAuthClientId("demo_api_swagger");
                    options.OAuthAppName("Demo API - Swagger");
                    options.OAuthUsePkce();
                });

            }

            //First Time User Seed
            //IdentityDbInitializer.SeedData(userManager, roleManager);

            app.UseHttpsRedirection();

            app.UseRouting();


            //Enable CORS (Cross Orign Resource Sharing)//http://localhost:3000 
            app.UseCors(options =>
                options.WithOrigins("http://localhost:3000").AllowAnyMethod());
            app.UseCors("AllowOrigin");


            app.UseAuthentication();

            app.UseAuthorization();



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //.RequireAuthorization("ApiScope");
            });
        }
    }
}
