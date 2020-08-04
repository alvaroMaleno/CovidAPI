using System.Collections.Generic;
using CoVid.Models;
using System.Threading;
using CoVid.Models.OutputModels;
using CoVid.Utils;
using Covid_REST.Utils;

namespace CoVid.Cache
{
    public class CovidCache
    {
        private List<GeoZone> _oAllGeoZoneList;
        private Dictionary<string, ulong> _oAllDatesDic;
        private static CovidCache _instance = new CovidCache();

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
                this.POSTToDAOGetAllGeoZoneForAllDates(ref _oAllGeoZoneList);
                Thread.Sleep(UtilsConstants.IntConstants.MS_IN_A_DAY);
            }
        }

        private void CompleteDatesDictionary()
        {
            
            _oAllDatesDic = new Dictionary<string, ulong>();
            List<CovidDate> oCovidDateList = null;
            
            this.POSTToDAOGetAllDates(ref oCovidDateList);

            foreach (var oCovidDate in oCovidDateList)
            {
                if(_oAllDatesDic.ContainsKey(oCovidDate.date))
                    continue;
                
                _oAllDatesDic.Add(oCovidDate.date, oCovidDate.id);
            }
        }

        private void POSTToDAOGetAllDates(ref List<CovidDate> oCovidDateList)
        {
            DAOModelPOST oDAOModelPOST = new DAOModelPOST(UtilsConstants.POSTMethodsConstants.GET_ALL_DATES, null);
            oCovidDateList = UtilsJSON.GetInstance().DeserializeFromPOSTUrl(
                oCovidDateList, UtilsConstants.UrlConstants.URL_DATA_REST, oDAOModelPOST).Result; 
        }

        public void GetListFilteredByDate(string pStartDate, string pEndDate, out object pObjectToReturn)
        {
            if(_oAllDatesDic is null || _oAllDatesDic.Count < UtilsConstants.IntConstants.ONE)
                this.CompleteDatesDictionary();
            
            if(_oAllGeoZoneList is null || _oAllGeoZoneList.Count < UtilsConstants.IntConstants.ONE)
                this.POSTToDAOGetAllGeoZoneForAllDates(ref _oAllGeoZoneList);

            this.FillObjectToReturn(pStartDate, pEndDate, out pObjectToReturn);
        }

        private void FillObjectToReturn(string pStartDate, string pEndDate, out object pObjectToReturn)
        {
            ulong startDate = this.GetStartDate(pStartDate);
            ulong endDate = this.GetEndDate(pEndDate);

            GeoZone oGeoZoneToReturn;
            List<GeoZone> oListToFill = new List<GeoZone>();
            foreach (var oGeoZone in this._oAllGeoZoneList)
            {
                oGeoZoneToReturn = new GeoZone(oGeoZone, false);
                oGeoZoneToReturn.dataList = new List<CoVidData>();

                foreach (var oData in oGeoZone.dataList)
                    if(oData.id >= startDate && oData.id <= endDate)
                        oGeoZoneToReturn.dataList.Add(oData);
                
                oListToFill.Add(oGeoZoneToReturn);
            }

            pObjectToReturn = oListToFill;
        }

        private void POSTToDAOGetAllGeoZoneForAllDates(ref List<GeoZone> pAllGeoZoneList)
        {
            DAOModelPOST oDAOModelPOST = new DAOModelPOST(
                    UtilsConstants.POSTMethodsConstants.GET_ALL_GEO_ZONE_FOR_ALL_DATES, 
                    null);
            pAllGeoZoneList = UtilsJSON.GetInstance().DeserializeFromPOSTUrl(
                pAllGeoZoneList, UtilsConstants.UrlConstants.URL_DATA_REST, oDAOModelPOST).Result;
        }

        private ulong GetEndDate(string pEndDate)
        {
            if(_oAllDatesDic.ContainsKey(pEndDate))
                return _oAllDatesDic[pEndDate];
            
            return ulong.Parse(_oAllDatesDic.Count.ToString());
        }

        private ulong GetStartDate(string pStartDate)
        {
            if(_oAllDatesDic.ContainsKey(pStartDate))
                return _oAllDatesDic[pStartDate];
            
            return UtilsConstants.IntConstants.ONE;
        }
    }
}