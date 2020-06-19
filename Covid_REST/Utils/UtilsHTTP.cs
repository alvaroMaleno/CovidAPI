using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CoVid.Utils;

namespace Covid_REST.Utils
{
    public class UtilsHTTP
    {
        private static UtilsHTTP _instance;
        private UtilsHTTP(){}

        public static UtilsHTTP GetInstance()
        {
            if(_instance is null)
                _instance = new UtilsHTTP();
            return _instance;
        }

        public async Task<string> POSTJsonAsyncToURL<Input>(string pUrl, Input pJsonObject)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            using (var client = new HttpClient(clientHandler))
            {
                var response = await client.PostAsync(
                    pUrl, 
                    new StringContent(UtilsJSON.GetInstance().Serialize(pJsonObject), Encoding.UTF8, "application/json"));
                
                return await response.Content.ReadAsStringAsync();
            }
        } 
    }
}