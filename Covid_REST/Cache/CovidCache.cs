using Microsoft.VisualBasic.CompilerServices;
using System.Collections.Generic;
using CoVid.Models;
using System.Threading;
using CoVid.Models.OutputModels;
using CoVid.Utils;

namespace CoVid.Cache
{
    public class CovidCache
    {
        private List<GeoZone> _oAllGeoZoneList;
        private Dictionary<string, ulong> _oAllDatesDic;
        private static CovidCache _instance = new CovidCache();
        private readonly string _URL_DATA_REST = "https://localhost:5005/CovidDataBase";

        public static CovidCache GetInstance()
        {
            return _instance;
        }

        private CovidCache()
        {
            Thread oThread = new Thread(
                new ThreadStart(Refresh));
            oThread.Start();
        }

        private void Refresh()
        {
            while(true)
            {
                this.CompleteDatesDictionary();
                _oAllGeoZoneList = null;
                DAOModelPOST oDAOModelPOST = new DAOModelPOST("GetAllGeoZoneDataForAllDates", null);
                _oAllGeoZoneList = UtilsJSON.GetInstance().DeserializeFromPOSTUrl(
                    _oAllGeoZoneList, _URL_DATA_REST, oDAOModelPOST).Result;
                //Miliseconds in a Day
                Thread.Sleep(60 * 60 * 24 * 1000);
            }
        }

        private void CompleteDatesDictionary()
        {
            DAOModelPOST oDAOModelPOST = new DAOModelPOST("GetAllDates", null);
            this._oAllDatesDic = new Dictionary<string, ulong>();
            List<CovidDate> oCovidDateList = null;

            oCovidDateList = UtilsJSON.GetInstance().DeserializeFromPOSTUrl(oCovidDateList, _URL_DATA_REST, oDAOModelPOST).Result; 

            foreach (var oCovidDate in oCovidDateList)
            {
                if(this._oAllDatesDic.ContainsKey(oCovidDate.date))
                    continue;
                
                this._oAllDatesDic.Add(oCovidDate.date, oCovidDate.id);
            }
        }

        public void GetListFilteredByDate(string pStartDate, string pEndDate, out object objectToReturn)
        {
            if(_oAllDatesDic is null || _oAllDatesDic.Count < 1)
                this.CompleteDatesDictionary();
            
            ulong startDate = this._oAllDatesDic[pStartDate];
            ulong endDate = this._oAllDatesDic[pEndDate];
            GeoZone oGeoZoneToReturn;
            List<GeoZone> oListToFill = new List<GeoZone>();

            if(_oAllGeoZoneList is null || _oAllGeoZoneList.Count < 1)
            {
                DAOModelPOST oDAOModelPOST = new DAOModelPOST("GetAllGeoZoneDataForAllDates", null);
                _oAllGeoZoneList = UtilsJSON.GetInstance().DeserializeFromPOSTUrl(
                    _oAllGeoZoneList, _URL_DATA_REST, oDAOModelPOST).Result;
            }

            foreach (var oGeoZone in this._oAllGeoZoneList)
            {
                oGeoZoneToReturn = new GeoZone(oGeoZone, false);
                oGeoZoneToReturn.dataList = new List<CoVidData>();

                foreach (var oData in oGeoZone.dataList)
                    if(oData.id >= startDate && oData.id <= endDate)
                        oGeoZoneToReturn.dataList.Add(oData);
                
                oListToFill.Add(oGeoZoneToReturn);
            }
            objectToReturn = oListToFill;
        }
    }
}