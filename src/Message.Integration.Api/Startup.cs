using CacheManager.Core;
using Message.Integration.Aliyun.Clients;
using Message.Integration.Api.Filters;
using Message.Integration.Common.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;

namespace Message.Integration.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            InjectConfig(services);
            InjectRedis(services);
            InjectSwagger(services);
            InjectClients(services);

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ExceptionFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Message Integration V1");
                c.RoutePrefix = "swagger";
            });

            app.UseSwagger();

            app.UseMvc();
        }

        private void InjectConfig(IServiceCollection services)
        {
            services.Configure<AliyunSMSConfig>(Configuration.GetSection("AliyunSMSConfig"));
        }

        private void InjectRedis(IServiceCollection services)
        {
            var redisConfig = new RedisConfig();
            Configuration.GetSection("RedisConfig").Bind(redisConfig);

            var cacheManagerConfig =
                CacheManager.Core.ConfigurationBuilder.BuildConfiguration(settings =>
                {
                    settings.WithJsonSerializer()
                        .WithMicrosoftMemoryCacheHandle("memory")
                        .And
                        .WithRedisConfiguration("redis", redisConfig.Servers)
                        .WithRedisBackplane("redis")
                        .WithRedisCacheHandle("redis", true);
                });

            services.AddSingleton(cacheManagerConfig);
            services.AddSingleton(typeof(ICacheManager<>), typeof(BaseCacheManager<>));
        }

        private void InjectClients(IServiceCollection services)
        {
            services.AddTransient<IMessageClient, MessageClient>();
        }

        private void InjectSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info { Title = "Message Integration Api", Version = "v1" });

                var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, "Message.Integration.Api.xml");
                options.IncludeXmlComments(filePath);
            });
        }
    }
}