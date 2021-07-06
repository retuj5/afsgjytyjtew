﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Prometheus.Client.AspNetCore;
using Prometheus.Client.HttpRequestDurations;
using Promitor.Agents.Scraper.Configuration.Sinks;

// ReSharper disable once CheckNamespace
namespace Promitor.Agents.Scraper.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///     Adds the required metric sinks
        /// </summary>
        /// <param name="app">Application Builder</param>
        /// <param name="configuration">Configuration of the scraper agent</param>
        /// <param name="logger"></param>
        public static IApplicationBuilder UseMetricSinks(this IApplicationBuilder app, IConfiguration configuration, ILogger<Startup> logger)
        {
            var metricSinkConfiguration = configuration.GetSection("metricSinks").Get<MetricSinkConfiguration>();
            if (metricSinkConfiguration?.PrometheusScrapingEndpoint != null)
            {
                if (string.IsNullOrWhiteSpace(metricSinkConfiguration.PrometheusScrapingEndpoint.BaseUriPath) == false)
                {
                    logger.LogInformation("Adding Prometheus sink to expose on {PrometheusUrl}", metricSinkConfiguration.PrometheusScrapingEndpoint.BaseUriPath);
                    app.UsePrometheusRequestDurations(requestDurationsOptions =>
                    {
                        requestDurationsOptions.IncludePath = true;
                        requestDurationsOptions.IncludeMethod = true;
                        requestDurationsOptions.MetricName = "promitor_runtime_http_request_duration_seconds";
                    });
                    app.UsePrometheusServer(prometheusOptions =>
                    {
                        prometheusOptions.MapPath = metricSinkConfiguration.PrometheusScrapingEndpoint.BaseUriPath;
                        prometheusOptions.UseDefaultCollectors = false;
                    });
                }
            }

            return app;
        }
    }
}
