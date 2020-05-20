using System.Threading;
using System.Threading.Tasks;
using CoVid.Controllers;
using CoVid.Models.PathModels;
using CoVid.Processes;
using CoVid.Processes.InitialCreateTables;
using CoVid.Processes.InitialDataInsertion;
using CoVid.Processes.Threading;
using CoVid.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace CoVid
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

        public static void InizializeInitialProceses()
        {
            var oInitialCreateTables = InitialCreateTables.GetInstance(
                CovidDAOPostgreImpl.GetInstance());
            InitialDataInsertions oInitialInsertions = new InitialDataInsertions(
                CovidDAOPostgreImpl.GetInstance(), true);
            TaskableThreadManager oTaskableThreadManager = new TaskableThreadManager(
                true, oInitialCreateTables, oInitialInsertions);
            oTaskableThreadManager.ThreadProc();
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
