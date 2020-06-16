using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CoVid.Models;

namespace CoVid.Utils
{
    public class UtilsCovidDateManagement
    {
        private static UtilsCovidDateManagement _instance;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static UtilsCovidDateManagement GetInstance(){
            if(_instance is null){
                _instance = new UtilsCovidDateManagement();
            }
            return _instance;
        }
        private UtilsCovidDateManagement(){

        }

        public async Task CompleteCovidDatesDictionary(
            ConcurrentDictionary<string, CovidDate> oDateKeyCovidDateValue, 
            List<GeoZone> oGeoZonesList)
        {
            Task[] oTaskArray = new Task[200];
            int taskCounter = oTaskArray.Length - 1;
            bool hasBeenCompletedOneTime = false;

            foreach (var oGeoZone in oGeoZonesList)
            {
                if(!hasBeenCompletedOneTime)
                {
                    oTaskArray[taskCounter--] = this.AddDateToConcurrentDictionary(oGeoZone, oDateKeyCovidDateValue);
                }
                else
                {
                    await oTaskArray[taskCounter];
                    oTaskArray[taskCounter--] = this.AddDateToConcurrentDictionary(oGeoZone, oDateKeyCovidDateValue);
                }
                if(taskCounter < 0)
                {
                    hasBeenCompletedOneTime = true;
                    taskCounter = oTaskArray.Length - 1;
                }
            }
        }

        private async Task AddDateToConcurrentDictionary(
            GeoZone oGeoZone, 
            ConcurrentDictionary<string, CovidDate> pDateKeyCovidDateValue)
        {
            foreach (var oData in oGeoZone.dataList)
            {
                if(!pDateKeyCovidDateValue.ContainsKey(oData.date.date))
                    pDateKeyCovidDateValue.GetOrAdd(oData.date.date, oData.date);
            }
        }
    }
}