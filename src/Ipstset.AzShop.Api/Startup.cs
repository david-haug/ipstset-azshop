using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Ipstset.Auth.JwtTokens;
using Ipstset.Auth.JwtTokens.Attributes;
using Ipstset.AzShop.Api.Attributes;
using Ipstset.AzShop.Api.Logging;
using Ipstset.AzShop.Api.Tokens;
using Ipstset.AzShop.Application;
using Ipstset.AzShop.Application.Behaviors;
using Ipstset.AzShop.Application.EventHandling;
using Ipstset.AzShop.Application.Products;
using Ipstset.AzShop.Application.Shops;
using Ipstset.AzShop.Application.Shops.CreateShop;
using Ipstset.AzShop.Domain.Products;
using Ipstset.AzShop.Domain.Shops;
using Ipstset.AzShop.Infrastructure.SqlDataAccess;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Ipstset.AzShop.Api
{
    public class Startup
    {
        private string _contentRoot;
        private string _authUrl;
        private string _azShopConnection;
        private JwtTokenSettings _jwtTokenSettings;

        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            _contentRoot = env.ContentRootPath;

            _authUrl = Environment.GetEnvironmentVariable("AUTH_URL");
            _azShopConnection = Environment.GetEnvironmentVariable("AZSHOP_CONNECTION");

            _jwtTokenSettings = new JwtTokenSettings
            {
                Issuers = Configuration["JwtTokenSettings:Issuers"].Split(","),
                Audiences = Configuration["JwtTokenSettings:Audiences"].Split(","),
                MinutesToExpire = Convert.ToInt32(Configuration["JwtTokenSettings:MinutesToExpire"]),
                Secret = Environment.GetEnvironmentVariable("JWT_TOKEN_SECRET")
            };
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(
                    options =>
                    {
                        options.Filters.Add(typeof(LogRequestServiceFilter));
                        options.Filters.Add(typeof(AuthenticateJwtTokenAttribute));
                    }
                )
                .AddNewtonsoftJson()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateShopValidator>());

            #region Repository injection

            var db = new DbSettings
            {
                Connection = _azShopConnection,
                Schema = Configuration["DbSettings:Schema"],
            };

            services.AddTransient<IEventDispatcher, EventDispatcher>();
            services.AddTransient<IEventRepository, EventRepository>((ctx) => new EventRepository(db));

            services.AddTransient<IShopRepository, ShopRepository>((ctx) =>
            {
                var dispatcher = ctx.GetService<IEventDispatcher>();
                return new ShopRepository(db, dispatcher);
            });
            services.AddTransient<IShopReadOnlyRepository, ShopReadOnlyRepository>((ctx) => new ShopReadOnlyRepository(db));

            services.AddTransient<IProductRepository, ProductRepository>((ctx) =>
            {
                var dispatcher = ctx.GetService<IEventDispatcher>();
                return new ProductRepository(db, dispatcher);
            });
            services.AddTransient<IProductReadOnlyRepository, ProductReadOnlyRepository>((ctx) => new ProductReadOnlyRepository(db));
            services.AddTransient<ILogRepository, LogRepository>((ctx) => new LogRepository(db));

            #endregion

            //AppUser for event logging
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient(u =>
                TokenClaimsService.CreateAppUser(new HttpContextAccessor().HttpContext?.User?.Claims));
            services.AddTransient<IJwtTokenManager, JwtTokenManager>((ctx) => new JwtTokenManager(_jwtTokenSettings));

            #region Mediatr
            services.AddMediatR(typeof(CreateShopHandler).GetTypeInfo().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            #endregion

            //CORS
            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());

                options.DefaultPolicyName = "CorsPolicy";
            });

            //RESPONSE JSON FORMATTING
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ContractResolver =
                            new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    }
                );

            #region swagger

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AzShop API", Version = "v1" });
            //    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
            //    {
            //        Flows = new OpenApiOAuthFlows
            //        {
            //            Password = new OpenApiOAuthFlow
            //            {
            //                AuthorizationUrl = new Uri(_authUrl),
            //                TokenUrl = new Uri(_authUrl + "/connect/token"),
            //                Scopes = new Dictionary<string, string>
            //                {
            //                    {"ottajot_api", "Otta Jot API"},
            //                }

            //            }
            //        },
            //        In = ParameterLocation.Header,
            //        Name = "Authorization",
            //        Type = SecuritySchemeType.OAuth2
            //    });
            //    c.OperationFilter<AuthenticationRequirementsOperationFilter>();
            //    var filePath = Path.Combine(AppContext.BaseDirectory, "Ipstset.AzShop.Api.xml");
            //    c.IncludeXmlComments(filePath);
            //});

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //needed for Heroku
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });

            //app.UseSwagger();
            //app.UseSwaggerUI(c =>
            //    {
            //        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AZ Shop API V1");
            //        c.OAuthClientId("swagger_ui");
            //        c.OAuthAppName("AZ Shop API - Swagger");
            //        c.OAuthClientSecret("swagger_secret");
            //    }
            //);
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            //app.UseAuthentication();
            //app.UseAuthorization();

            //for wwwroot content
            app.UseStaticFiles();

            //app.Map("/auth", builder =>
            //{
            //    builder.UseIdentityServer();
            //    builder.UseMvcWithDefaultRoute();
            //});

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        }
    }

    //public class AuthenticationRequirementsOperationFilter : IOperationFilter
    //{
    //    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    //    {
    //        if (operation.Security == null)
    //            operation.Security = new List<OpenApiSecurityRequirement>();

    //        var scheme = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" } };
    //        operation.Security.Add(new OpenApiSecurityRequirement
    //        {
    //            [scheme] = new List<string>()
    //        });
    //    }
    //}
}
