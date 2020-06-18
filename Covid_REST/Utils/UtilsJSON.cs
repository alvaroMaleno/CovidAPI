using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
            HttpClient oHttpClient = new HttpClient();
            pTargetClass = System.Text.Json.JsonSerializer.Deserialize<R>(
                oHttpClient.GetAsync(pUrl).Result.Content.ReadAsStringAsync().Result);
            oHttpClient = null;
        }

        public async Task<R> DeserializeFromPOSTUrl<R, Input>(R pTargetClass, string pUrl, Input pJsonObject){
            
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                var response = await client.PostAsync(
                    pUrl, 
                    new StringContent(this.Serialize(pJsonObject), Encoding.UTF8, "application/json"));
                
                return System.Text.Json.JsonSerializer.Deserialize<R>(await response.Content.ReadAsStringAsync());
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