using System.Collections.Generic;
using CoVid.DAO;
using CoVid.DAOs.Abstracts;
using CoVid.Models;
using System.Threading;

namespace AccesoADatos_REST.Cache
{
    public class CovidCache
    {
        private CovidDAO _oCovidDAO = CovidDAOPostgreImpl.GetInstance();
        private List<GeoZone> _oAllGeoZoneList;
        private static CovidCache _instance;

        public static CovidCache GetInstance()
        {
            if(_instance is null)
                _instance = new CovidCache();
            
            return _instance;
        }

        private CovidCache()
        {
            _oAllGeoZoneList = new List<GeoZone>();
            _oCovidDAO.GetAllGeoZoneDataForAllDates(_oAllGeoZoneList);
            Thread oThread = new Thread(
                new ThreadStart(Refresh));
            oThread.Start();
        }

        private void Refresh()
        {
            while(true)
            {
                _oAllGeoZoneList = new List<GeoZone>();
                _oCovidDAO.GetAllGeoZoneDataForAllDates(_oAllGeoZoneList);
                //Miliseconds in a Day
                Thread.Sleep(60 * 60 * 24 * 1000);
            }
        }

        public List<GeoZone> GetCompleteList()
        {
            return this._oAllGeoZoneList;
        }
    }
}