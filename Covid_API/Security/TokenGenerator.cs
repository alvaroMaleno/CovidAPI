using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CoVid.Models.InputModels;
using Covid_REST.Utils;
using Microsoft.IdentityModel.Tokens;

namespace CoVid.Security
{

    public class TokenGenerator
    {
        public static void GenerateTokenFromUser(User pUser, out JwtSecurityToken pJwtSecurityToken)
        {
            string toSecretKey = UtilsHTTP.GetInstance().GetFromUrl(UtilsConstants.UrlConstants.URL_SECURITY_REST);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, pUser.public_key),
		        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            pJwtSecurityToken = new JwtSecurityToken
            (
                issuer: "covidTokenIssuer",
                audience: "covidTokenAudience",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(UtilsConstants.IntConstants.HOURS_IN_A_DAY),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(toSecretKey)), SecurityAlgorithms.HmacSha256)
            );
        }
    }
}