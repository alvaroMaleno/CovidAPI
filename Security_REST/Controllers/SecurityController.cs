using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Security_REST.Models.DataModels;
using Security_REST.Security.SecurityManager;

namespace API_Security.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecurityController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private SecurityManager _oSecurityManager = SecurityManager.GetInstance();

        public SecurityController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public object Get()
        {
            KeyPair oKeyPair;
            _oSecurityManager.GetAPIKeyPair(out oKeyPair);
            return oKeyPair.public_string;
        }
    }
}
