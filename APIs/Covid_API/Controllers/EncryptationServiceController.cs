using System.Threading.Tasks;
using Covid_REST.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Security_REST.Models.DataModels;

namespace API_Security.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class EncryptationServiceController : ControllerBase
    {

        private readonly ILogger<EncryptationServiceController> _logger;

        public EncryptationServiceController(ILogger<EncryptationServiceController> logger)
        {
            _logger = logger;
        }


        [HttpPost]
        public string Post(ToEncrypt pToEncrypt)
        {
            return this.SendToEncryptationService(pToEncrypt).Result;
        }

        private async Task<string> SendToEncryptationService(ToEncrypt pToEncrypt)
        {
            return await UtilsHTTP.GetInstance().POSTJsonAsyncToURL(
                UtilsConstants.UrlConstants.URL_SECURITY_ENCRIPTATION_REST, 
                pToEncrypt);
        }
    }
}
