using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;

namespace CoVid.Utils
{
    public class UtilsJSON
    {
        private static UtilsJSON _instance;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static UtilsJSON GetInstance(){
            if(_instance is null){
                _instance = new UtilsJSON();
            }
            return _instance;
        }
        private UtilsJSON(){

        }

        public void DeserializeFromString<R>(out R pTargetClass, string pStringToDeserialize){
            pTargetClass = System.Text.Json.JsonSerializer.Deserialize<R>(pStringToDeserialize);
        }

        public void DeserializeFromUrl<R>(out R pTargetClass, string pUrl){
            try
            {
                HttpClient oHttpClient = new HttpClient();
                var result = oHttpClient.GetAsync(pUrl).Result.Content.ReadAsStringAsync().Result;
                pTargetClass = System.Text.Json.JsonSerializer.Deserialize<R>(result);
                oHttpClient = null;
            }
            catch(Exception)
            {
                pTargetClass = default(R);
            }
        }

        public void JsonParseJArrayFromUrl(out JArray pObject, string pUrl)
        {
            HttpClient oHttpClient = new HttpClient();
            var json = oHttpClient.GetAsync(pUrl).Result.Content.ReadAsStringAsync().Result;
            try
            {
                pObject = JArray.Parse(json);
            }
            catch (System.Exception)
            {
                pObject = null;
            }
              
            oHttpClient = null;
        }

        public string Serialize<R>(R pObjectToSerialize)
        {
            return System.Text.Json.JsonSerializer.Serialize<R>(pObjectToSerialize);
        }
    }
}