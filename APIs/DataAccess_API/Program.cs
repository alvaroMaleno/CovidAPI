using System.Collections.Generic;
using System.Threading;
using CoVid.DAO;
using CoVid.Models;
using CoVid.Processes.InitialDataInsertion;
using CoVid.Processes.Interfaces;
using CoVid.Processes.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using CoVid.Processes.InitialCreateTables;

namespace API_DAO
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
            bool isNecessaryCreateTables;
            isNecessaryCreateTables = CheckTables();

            List<ITaskable> oITaskableList = new List<ITaskable>();
            if(isNecessaryCreateTables)
            {
                oITaskableList.Add(InitialCreateTables.GetInstance(
                                    CovidDAOPostgreImpl.GetInstance()));
            }
            oITaskableList.Add(new InitialDataInsertions(CovidDAOPostgreImpl.GetInstance(), true));

            TaskableThreadManager oTaskableThreadManager = new TaskableThreadManager(true, oITaskableList.ToArray());
                
            oTaskableThreadManager.ThreadProc();
        }

        private static bool CheckTables()
        {
            CovidDAOPostgreImpl oCovidDAO = CovidDAOPostgreImpl.GetInstance();
            List<GeoZone> oGeoZoneList = new List<GeoZone>();
            oCovidDAO.GetAllCountries(oGeoZoneList);
            if(oGeoZoneList.Count < 200)
            {
                return true;
            }
            List<CovidDate> oCovidDateList = new List<CovidDate>();
            oCovidDAO.GetAllDates(oCovidDateList);
            if(oCovidDateList.Count < 100)
            {
                return true;
            }

            return false;
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
		    webBuilder.UseUrls("http://*:5006");
                });
    }
}
