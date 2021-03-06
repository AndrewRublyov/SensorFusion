﻿using System.Threading;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace SensorFusion.Web.Receiver
{
  public class Program
  {
    public static void Main(string[] args)
    {
      SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
      CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
      WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .ConfigureLogging((hostingContext, logging) =>
        {
          logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
          logging.AddConsole();
          logging.AddDebug();
          logging.AddEventSourceLogger();
        });
  }
}