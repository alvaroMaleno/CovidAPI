using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Security_REST.DAOs.Processes.InitialTableCreation;

namespace API_Security
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Thread oThread = new Thread(
                new ThreadStart(InizializeInitialProceses));
            oThread.Start();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void InizializeInitialProceses()
        {
            InitialTableCreator.GetInstance().InitialTableCreation();
        }
    }

}
