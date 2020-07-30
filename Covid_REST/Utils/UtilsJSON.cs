using Microsoft.VisualBasic.CompilerServices;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Covid_REST.Utils;
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
            pTargetClass = System.Text.Json.JsonSerializer.Deserialize<R>(
                UtilsHTTP.GetInstance().GetFromUrl(pUrl));
        }

        public async Task<R> DeserializeFromPOSTUrl<R, Input>(R pTargetClass, string pUrl, Input pJsonObject){
            
            return System.Text.Json.JsonSerializer.Deserialize<R>(
                await UtilsHTTP.GetInstance().POSTJsonAsyncToURL(pUrl, pJsonObject));
        }

        public void JsonParseJArrayFromUrl(out JArray pObject, string pUrl)
        {
            var oJson = UtilsHTTP.GetInstance().GetFromUrl(pUrl);;
            try
            {
                pObject = JArray.Parse(oJson);
            }
            catch (System.Exception)
            {
                pObject = new JArray();
            }
        }

        public string Serialize<R>(R pObjectToSerialize)
        {
            return System.Text.Json.JsonSerializer.Serialize<R>(pObjectToSerialize);
        }
    }
}