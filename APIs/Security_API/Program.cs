using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
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
