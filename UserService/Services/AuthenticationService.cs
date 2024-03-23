using JWT.Algorithms;
using JWT;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Common;
using JWT.Serializers;
using UserService.Domain.Common;
using JWT.Builder;

namespace UserService.Services
{
    
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtAlgorithm _algorithm;
        private readonly IJsonSerializer _serializer;
        private readonly IBase64UrlEncoder _base64Encoder;
        private readonly IJwtEncoder _jwtEncoder;
        public AuthenticationService()
        {
            // JWT specific initialization.
            _algorithm = new HMACSHA256Algorithm();
            _serializer = new JsonNetSerializer();
            _base64Encoder = new JwtBase64UrlEncoder();
            _jwtEncoder = new JwtEncoder(_algorithm, _serializer, _base64Encoder);
        }
        public string IssueJWT(string user)
        {
            var authSecret = Environment.GetEnvironmentVariable("AuthSecret");
            var token = TokenDetails.GetExistingToken(user);
            if (string.IsNullOrEmpty(token))
            {
                Dictionary<string, object> claims = new Dictionary<string, object> {
                    // JSON representation of the user Reference with ID and display name
                    {
                        "username",
                        user
                    }
                 };
                token = _jwtEncoder.Encode(claims, authSecret); // Put this key in config
                TokenDetails.AddTokenDetails(user, token);
            }
            return token;
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
    }
}
