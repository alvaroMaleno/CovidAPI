using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Security_REST.Models.DataModels;
using Security_REST.Security.SecurityManager;
using Security_REST.Utils;

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

        [HttpPost]
        public object Post(User pUser)
        {
            if(this.ThereArePostError(pUser))
                return new Microsoft.AspNetCore.Mvc.BadRequestResult();
            
            if(pUser?.newUser != true)
                return _oSecurityManager.ValidateUser(pUser);

            _oSecurityManager.AddUser(pUser);

            if(pUser.public_key == UtilsConstants._PLEASE_ENCRYPT_ERROR)
                return UtilsConstants._PLEASE_ENCRYPT_ERROR;
            
            return pUser.public_key;
        }

        private bool ThereArePostError(User pUser)
        {
            if(pUser is null)
                return true;
            if(string.IsNullOrEmpty(pUser.email) || string.IsNullOrEmpty(pUser.pass))
                return true;
            return false;
        }
    }
}
