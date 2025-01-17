﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Opsi.ComingSoon.IO.AspNet;
using Opsi.ComingSoon.Data.AspNet;

namespace Opsi.ComingSoon.UI.Server
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    public Startup(IHostEnvironment env)
    {
      Configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors();
      services.Add(ServiceDescriptor.Singleton(Configuration));
      services.AddMvcCore()
        .AddOpsiCommingSoonApi();

      services.AddOpsiComingSoonData();

      services.AddSpaStaticFiles(configuration: options => { options.RootPath = "wwwroot"; });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostEnvironment env, ILoggerFactory loggerFactory, IHostApplicationLifetime appLifetime)
    {
      app.UseCors(config => config
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowAnyOrigin());

      app.UseDefaultFiles();
      app.UseSpaStaticFiles(new StaticFileOptions
      {
        OnPrepareResponse = (context) => {
          context.Context.Response.Headers["Cache-Control"] = context.Context.Request.Path.Value == "/index.html"
            ? "no-cache, no-store"
            : "max-age=31536000";
        }
      });

      app.UseRouting();
      app.UseEndpoints(endpoints => endpoints.MapControllers());

      app.UseSpa(builder => {
        if (env.IsDevelopment())
        {
          builder.UseProxyToSpaDevelopmentServer(baseUri: "http://localhost:4200");
        }
      });
    }
  }
}
