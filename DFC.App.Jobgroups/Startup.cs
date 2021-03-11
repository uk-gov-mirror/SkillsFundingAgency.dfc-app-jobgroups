using AutoMapper;
using DFC.App.JobGroups.Data.Contracts;
using DFC.App.JobGroups.Data.Models.ClientOptions;
using DFC.App.JobGroups.Data.Models.ContentModels;
using DFC.App.JobGroups.Data.Models.JobGroupModels;
using DFC.App.JobGroups.HostedServices;
using DFC.App.JobGroups.Services.CacheContentService;
using DFC.App.JobGroups.Services.CacheContentService.Connectors;
using DFC.App.JobGroups.Services.CacheContentService.Webhooks;
using DFC.Compui.Cosmos;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Subscriptions.Pkg.Netstandard.Extensions;
using DFC.Compui.Telemetry;
using DFC.Content.Pkg.Netcore.Data.Models.PollyOptions;
using DFC.Content.Pkg.Netcore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.JobGroups
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private const string AppSettingsPolicies = "Policies";
        private const string CosmosDbJobGroupConfigAppSettings = "Configuration:CosmosDbConnections:JobGroup";
        private const string CosmosDbSharedContentConfigAppSettings = "Configuration:CosmosDbConnections:SharedContent";

        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.configuration = configuration;
            this.env = env;
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                // add the default route
                endpoints.MapControllerRoute("default", "{controller=Health}/{action=Ping}");
            });
            mapper?.ConfigurationProvider.AssertConfigurationIsValid();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var cosmosDbConnectionJobGroup = configuration.GetSection(CosmosDbJobGroupConfigAppSettings).Get<CosmosDbConnection>();
            var cosmosDbConnectionSharedContent = configuration.GetSection(CosmosDbSharedContentConfigAppSettings).Get<CosmosDbConnection>();
            services.AddDocumentServices<JobGroupModel>(cosmosDbConnectionJobGroup, env.IsDevelopment());
            services.AddDocumentServices<ContentItemModel>(cosmosDbConnectionSharedContent, env.IsDevelopment());

            services.AddApplicationInsightsTelemetry();
            services.AddHttpContextAccessor();
            services.AddHostedServiceTelemetryWrapper();
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddHostedService<SharedContentCacheReloadBackgroundService>();
            services.AddSubscriptionBackgroundService(configuration);
            services.AddTransient<IJobGroupCacheRefreshService, JobGroupCacheRefreshService>();
            services.AddTransient<IWebhooksService, WebhooksService>();
            services.AddTransient<IWebhooksContentService, WebhooksContentService>();
            services.AddTransient<IWebhooksDeleteService, WebhooksDeleteService>();
            services.AddTransient<ISharedContentCacheReloadService, SharedContentCacheReloadService>();
            services.AddTransient<IApiConnector, ApiConnector>();
            services.AddTransient<IApiDataConnector, ApiDataConnector>();

            var policyOptions = configuration.GetSection(AppSettingsPolicies).Get<PolicyOptions>() ?? new PolicyOptions();
            var policyRegistry = services.AddPolicyRegistry();

            services.AddSingleton(configuration.GetSection(nameof(LmiTransformationApiClientOptions)).Get<LmiTransformationApiClientOptions>() ?? new LmiTransformationApiClientOptions());
            services.AddApiServices(configuration, policyRegistry);

            services
                .AddPolicies(policyRegistry, nameof(LmiTransformationApiClientOptions), policyOptions)
                .AddHttpClient<ILmiTransformationApiConnector, LmiTransformationApiConnector, LmiTransformationApiClientOptions>(nameof(LmiTransformationApiClientOptions), nameof(PolicyOptions.HttpRetry), nameof(PolicyOptions.HttpCircuitBreaker));

            services.AddMvc(config =>
                {
                    config.RespectBrowserAcceptHeader = true;
                    config.ReturnHttpNotAcceptable = true;
                })
                .AddNewtonsoftJson()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }
    }
}