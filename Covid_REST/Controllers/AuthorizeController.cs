using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CoVid.Cache;
using CoVid.Models.InputModels;
using CoVid.Models.OutputModels;
using CoVid.Utils;
using Covid_REST.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CoVid.Controllers
{

    [Route("api/[controller]")]
    public class Authorize : Controller
    {
        private readonly string _URL_SECURITY_REST = "https://localhost:5003/Security";

        [HttpPost]
        public object Post([FromBody]User pUser)
        {
            var isAuthenticated = this.AuthenticateUser(pUser).Result;
            if(!isAuthenticated.Contains("true"))
            {
                return "User/Pass Error";
            }

            string toSecretKey = UtilsJSON.GetInstance().GetFromUrl(_URL_SECURITY_REST);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, pUser.public_key),
		        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken
            (
                issuer: "covidTokenIssuer",
                audience: "covidTokenAudience",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(toSecretKey)),  SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }


        private async Task<string> AuthenticateUser(Models.InputModels.User pUser)
        {
            return await UtilsHTTP.GetInstance().POSTJsonAsyncToURL(this._URL_SECURITY_REST, pUser);
        }

    }
}