using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoVid.Controllers;
using CoVid.DAOs.SelectTableOperations;
using CoVid.Models.InputModels;
using CoVid.Models.PathModels;
using CoVid.Processes;
using CoVid.Processes.InitialCreateTables;
using CoVid.Processes.InitialDataInsertion;
using CoVid.Processes.Threading;
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
            PostgreSqlSelect oPostgre = PostgreSqlSelect.GetInstance();
            List<string> countryList = new List<string>(){"AO", "AG"};
            Dates oDates = new Dates("01/05/2020", "06/05/2020");
            CovidData oCo = new CovidData(countryList, oDates, "");
            
            oPostgre.GetGeoZoneData(oCo, new List<Models.GeoZone>());
            
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
