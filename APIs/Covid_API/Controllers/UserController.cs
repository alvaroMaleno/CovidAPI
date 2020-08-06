using System.Threading.Tasks;
using CoVid.Models.InputModels;
using Covid_REST.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CoVid.Controllers
{

    [Route("api/[controller]")]
    public class UserController : Controller
    {
        [HttpGet]
        public object Get()
        {
            return UtilsHTTP.GetInstance().GetFromUrl(UtilsConstants.UrlConstants.URL_SECURITY_REST);
        }

        [HttpPost]
        public object Post([FromBody]User pUser)
        {
            return SendUserToSecurityService(pUser).Result;
        }

        private async Task<string> SendUserToSecurityService(User pUser)
        {
            return await UtilsHTTP.GetInstance().POSTJsonAsyncToURL(
                UtilsConstants.UrlConstants.URL_SECURITY_REST, 
                pUser);
        }

    }
}