using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EamaShop.Catalog.API.DTO;
using EamaShop.Catalog.API.Respository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace EamaShop.Catalog.API
{
    /// <summary>
    /// configer
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// init
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }
        /// <summary>
        /// config
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// env
        /// </summary>
        public IHostingEnvironment Environment { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // for restful api;
            services.AddMvcCore(Configure)
                .AddApiExplorer()
                .AddDataAnnotations()
                .AddJsonFormatters(Configure)
                .AddFormatterMappings()
                .AddCors(Configure)
                .AddAuthorization();

            // for authentication 
            services.AddAuthentication(Configure)
                .AddJwtBearer(Configure);

            // for cache
            services.AddResponseCaching();

            // for redis
            services.AddDistributedRedisCache(opt =>
            {
                opt.Configuration = Configuration["RedisConnectionConfiguration"];
                opt.InstanceName = Configuration["RedisInstanceName"];
            });

            services.AddDbContext<ProductContext>(opt =>
            {
                opt.UseNpgsql(Configuration.GetConnectionString("Master"));
            });

            services.TryAddSingleton<IMapper>(sp =>
            {
                Mapper.Initialize(Configure);
                return Mapper.Instance;
            });
            //services.AddDistributedRedisLock(opt =>
            //{
            //    opt.Configuration = Configuration["RedisConnectionConfiguration"];
            //    opt.InstanceName = Configuration["RedisInstanceName"];
            //});
            if (Environment.IsDevelopment())
            {
                services.AddSwaggerGen(opt =>
                {
                    var apiInfo = new Info()
                    {
                        Title = "Microservice of Catalog",
                        Version = "v1",
                        Description = $"The HTTP API Microservice of Catalog is designed as Data-Driven/CURD",
                        TermsOfService = "Terms Of Service"
                    };
                    opt.SwaggerDoc("v1", apiInfo);

                    opt.IgnoreObsoleteActions();
                    var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.xml", SearchOption.AllDirectories);
                    foreach (var f in files)
                    {
                        opt.IncludeXmlComments(f);
                    }
                    opt.OperationFilter<AuthorizeCheckOperationFilter>();
                });
            }
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {

            app.UseResponseCaching();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();

            if (Environment.IsDevelopment())
            {
                app.UseSwagger()
                .UseSwaggerUI(Configure);

                app.UseDeveloperExceptionPage();
            }
            ConfigureLogger(loggerFactory);

            try
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    scope.ServiceProvider
                        .GetRequiredService<ProductContext>()
                        .Database.EnsureCreated();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #region LoggerFactory
        private void ConfigureLogger(ILoggerFactory loggerFactory)
        {
            loggerFactory.AddNLog();
            var config = new LoggingConfiguration();
            var title = "${longdate}|事件Id=${event-properties:item=EventId.Id}|${logger}";
            var body = "${newline}${message}";
            var layout = title + body + "${newline}ErrorMessage: ${exception}${newline}####################################################################";

            // 普通日志
            var fileTarge = new FileTarget()
            {
                Layout = layout,
                ArchiveNumbering = ArchiveNumberingMode.Sequence,
                FileName = "../logs/${shortdate}/${level}.log",
                FileNameKind = FilePathKind.Relative,
                ArchiveFileKind = FilePathKind.Relative,
                ArchiveFileName = "../logs/${shortdate}/${level}-{####}.log",
                ArchiveEvery = FileArchivePeriod.None,
                ArchiveAboveSize = 1024 * 1024
            };

            // 微软日志
            var msTarge = new FileTarget()
            {
                Layout = "${level}" + layout,
                ArchiveNumbering = ArchiveNumberingMode.Sequence,
                FileName = "../logs/${shortdate}/Microsoft.log",
                FileNameKind = FilePathKind.Relative,
                ArchiveFileKind = FilePathKind.Relative,
                ArchiveFileName = "../logs/${shortdate}/${level}-{####}.log",
                ArchiveEvery = FileArchivePeriod.None,
                ArchiveAboveSize = 1024 * 1024
            };


            config.AddTarget("file", fileTarge);
            config.AddTarget("microsoft", msTarge);
            config.AddTarget("skip", new NullTarget());

            // 预发布和测试环境允许trace和debug
            if (Environment.IsDevelopment() || Environment.IsStaging())
            {
                config.AddRuleForOneLevel(NLog.LogLevel.Trace, "file");
                config.AddRuleForOneLevel(NLog.LogLevel.Debug, "file");
            }
            // 预发布允许信息级别
            if (Environment.IsStaging())
            {
                config.AddRuleForOneLevel(NLog.LogLevel.Info, "file");
            }
            // 允许应用
            config.AddRule(NLog.LogLevel.Warn, NLog.LogLevel.Off, "file", "EamaShop.*");
            // 微软的debug和trace日志跳过
            config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Debug, "skip", "Microsoft.*");
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Off, "microsoft", "Microsoft.*");
            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Off, "microsoft", "EntityFramework*");
            loggerFactory.ConfigureNLog(config);
        }
        #endregion

        #region Swagger Middleware
        private void Configure(SwaggerUIOptions options)
        {
            options.SwaggerEndpoint($"/swagger/v1/swagger.json", AppDomain.CurrentDomain.FriendlyName);
            options.ConfigureOAuth2("swaggerui", "", "", AppDomain.CurrentDomain.FriendlyName);
        }
        #endregion

        #region Mvc Configurations
        private void Configure(MvcOptions options)
        {
            var cacheProfile = new CacheProfile()
            {
                Duration = 30,
                Location = ResponseCacheLocation.Any,
                NoStore = false,
                VaryByQueryKeys = new[] { "*" }
            };
            options.CacheProfiles.Add("default", cacheProfile);

            options.Filters.Add<GlobalExceptionFilter>();
            options.Filters.Add<DomainExceptionFilter>();
        }
        #endregion

        #region Mvc Json Configurations
        private void Configure(JsonSerializerSettings options)
        {
            options.ContractResolver = null;
        }
        #endregion

        #region Mvc Cors Configurations
        private void Configure(CorsOptions options)
        {
            void Buil(CorsPolicyBuilder builder)
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowCredentials().AllowAnyHeader();
            }
            options.AddPolicy("CorsPolicy", Buil);
        }

        #endregion

        #region Authentication Configurations
        private void Configure(AuthenticationOptions options)
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;

        }
        #endregion

        #region JwtBearer Configurations
        private void Configure(JwtBearerOptions options)
        {
            var parameters = new TokenValidationParameters()
            {
                NameClaimType = ClaimTypes.Name,
                RoleClaimType = ClaimTypes.Role,
                ValidIssuer = ClaimsIdentity.DefaultIssuer,
                ValidAudience = EamaDefaults.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(EamaDefaults.JwtBearerSignKey)),
                TokenDecryptionKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(EamaDefaults.JwtBearerTokenKey))
            };

            options.TokenValidationParameters = parameters;
        }
        #endregion

        #region AutoMapper
        private void Configure(IMapperConfigurationExpression config)
        {
            config.CreateMap<ProductCreateDTO, Product>()
                .ForMember(pro => pro.PictureUris, opt => opt.MapFrom(dto => JsonConvert.SerializeObject(dto.PictureUris)))
                .ForMember(pro => pro.Properties, opt => opt.MapFrom(dto => JsonConvert.SerializeObject(dto.Properties)));
            config.CreateMap<SpecificationCreateDTO, Specification>()
                .ForMember(spec => spec.PictureUris, opt => opt.MapFrom(dto => JsonConvert.SerializeObject(dto.PictureUris)));
        }
        #endregion
    }
}
