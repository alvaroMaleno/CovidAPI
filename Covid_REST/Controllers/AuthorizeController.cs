using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using CoVid.Models.InputModels;
using CoVid.Security;
using Covid_REST.Utils;
using Microsoft.AspNetCore.Mvc;

namespace CoVid.Controllers
{

    [Route("api/[controller]")]
    public class Authorize : Controller
    {
        [HttpPost]
        public object Post([FromBody]User pUser)
        {
            var isAuthenticated = this.AuthenticateUser(pUser).Result;
            
            if(!isAuthenticated.Contains(UtilsConstants.StringConstants.TRUE))
                return "User/Pass Error";

            JwtSecurityToken oJwtSecurityToken;
            TokenGenerator.GenerateTokenFromUser(pUser, out oJwtSecurityToken);

            return new JwtSecurityTokenHandler().WriteToken(oJwtSecurityToken);
        }

        private async Task<string> AuthenticateUser(User pUser)
        {
            return await UtilsHTTP.GetInstance().POSTJsonAsyncToURL(
                UtilsConstants.UrlConstants.URL_SECURITY_REST, 
                pUser);
        }

    }
}