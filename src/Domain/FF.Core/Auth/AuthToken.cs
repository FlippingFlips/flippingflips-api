using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FF.Core.Auth
{
    public static class AuthToken
    {
        /// <summary>
        /// Creates a token with user claims / roles
        /// </summary>
        /// <param name="secretKey"></param>
        /// <param name="issuer"></param>
        /// <param name="audience"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="userEmail"></param>
        /// <param name="userRoles"></param>
        /// <param name="expireMinutes"></param>
        /// <returns></returns>
        public static string GetAccessToken(string secretKey, string issuer, 
            string audience, string userId, string userName, string userEmail,
            IList<string> userRoles, int expireMinutes = 60)
        {
            var token = string.Empty;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //add user claims and their roles
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, userEmail),
            };
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            //create JWT token
            var tokenOptions = new JwtSecurityToken(
                        issuer,
                        audience,
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                        signingCredentials: signIn);
            token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return token;
        }
    }
}
