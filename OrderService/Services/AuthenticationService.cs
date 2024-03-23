using JWT.Algorithms;
using JWT;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Common;
using JWT.Serializers;
using OrderService.Domain.Common;
using JWT.Builder;

namespace ProductService.Services
{
    
    public class AuthenticationService : IAuthenticationService
    {
        public AuthenticationService()
        {
        }

        public JWTResult ValidateToken(HttpRequest request)
        {
            var result = new JWTResult();
            var authSecret = Environment.GetEnvironmentVariable("AuthSecret");
            var expirationTime = Environment.GetEnvironmentVariable("ExpirationMinutes");

            // Check if we have a header.
            if (!request.Headers.ContainsKey("Authorization"))
            {
                result.IsValid = false;
                return result;
            }
            string authorizationHeader = request.Headers["Authorization"];
            
            // Check if the value is empty.
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                result.IsValid = false;
                return result;
            }
            // Check if we can decode the header.
            IDictionary<string, object> claims = null;
            try
            {
                if (authorizationHeader.StartsWith("Bearer"))
                {
                    authorizationHeader = authorizationHeader.Substring(7);
                }

                // Validate the token and decode the claims.
                claims = new JwtBuilder().WithAlgorithm(new HMACSHA256Algorithm()).WithSecret(authSecret).MustVerifySignature().Decode<IDictionary<string, object>>(authorizationHeader);
            }
            catch (Exception exception)
            {
                result.IsValid = false;
                return result;
            }
            // Check if we have user claim.
            if (!claims.ContainsKey("username"))
            {
                result.IsValid = false;
                return result;
            }



            result.Username = Convert.ToString(claims["username"]);
            result.IsValid = false;

            var existingTokenDetails = TokenDetails.GetTokenDetails(result.Username);

            if (existingTokenDetails != null && existingTokenDetails.Token.Equals(authorizationHeader) && existingTokenDetails.UserName.Equals(result.Username))
            {
                var currentTime = DateTime.UtcNow.AddMinutes(Convert.ToInt32(expirationTime));
                if (existingTokenDetails.GenerationTime >= currentTime)
                {
                    result.IsValid = true;
                }
            }
            return result;
        }

        public void AddToken(string user, string token)
        {
            TokenDetails.AddTokenDetails(user, token);
        }
    }
}
