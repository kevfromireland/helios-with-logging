using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using Owin;

namespace Helios.Logging.Example
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Run(async x =>
                          {
                              Trace.TraceInformation("Trace information message");

                              var azureListenerAssemblies = Trace.Listeners.Cast<TraceListener>()
                                  .Where(listener => listener.Name.Contains("Azure"))
                                  .Select(listener => listener.GetType().Assembly)
                                  .Select(assembly => assembly.CodeBase)
                                  .ToList();

                              x.Response.StatusCode = (int) HttpStatusCode.OK;

                              byte[] readAllBytes = File.ReadAllBytes(new Uri(azureListenerAssemblies.First()).LocalPath);

                              x.Response.ContentType = "application/octet-stream";
                              await x.Response.WriteAsync(readAllBytes);


                          });
        }
    }
}