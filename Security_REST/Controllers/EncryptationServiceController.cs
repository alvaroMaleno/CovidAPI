using Microsoft.VisualBasic.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Security_REST.Models.DataModels;
using Security_REST.Security;
using Security_REST.Security.SecurityManager;
using Security_REST.Utils;

namespace API_Security.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EncryptationServiceController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private SecurityManager _oSecurityManager = SecurityManager.GetInstance();

        public EncryptationServiceController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }


        [HttpPost]
        public string Post(ToEncrypt pToEncrypt)
        {
            if(this.ThereArePostError(pToEncrypt))
                return new Microsoft.AspNetCore.Mvc.BadRequestResult().ToString();
            
            return 
                RSAManager.GetInstance().EncryptWithPublicKeyString(pToEncrypt.text, pToEncrypt.key).ToString();
        }

        private bool ThereArePostError(ToEncrypt pToEncrypt)
        {
            if(pToEncrypt is null)
                return true;
            if(string.IsNullOrEmpty(pToEncrypt.key) || string.IsNullOrEmpty(pToEncrypt.text))
                return true;
            return false;
        }
    }
}
