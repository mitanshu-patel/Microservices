using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Common
{
    public static class TokenDetails
    {
        public static List<UserTokens> Tokens { get; set; } = new List<UserTokens>();

        public static string GetExistingToken(string user)
        {
            var expirationTime = Environment.GetEnvironmentVariable("ExpirationMinutes");
            var currentTime = DateTime.UtcNow.AddMinutes(Convert.ToInt32(expirationTime));
            var userToken = Tokens.FirstOrDefault(x => x.UserName.Equals(user));
            if (userToken != null && userToken.UserName.Equals(user))
            {
                if (userToken.GenerationTime >= currentTime)
                {
                    return userToken.Token;
                }
            }

            return string.Empty;
        }

        public static void AddTokenDetails(string user, string token)
        {
            var userToken = Tokens.FirstOrDefault(x => x.UserName.Equals(user));
            if(userToken == null)
            {
                userToken = new UserTokens {
                   UserName = user,
                   GenerationTime = DateTime.UtcNow,
                   Token = token
                };
                Tokens.Add(userToken);
            }

            userToken.GenerationTime = DateTime.UtcNow;
            userToken.Token = token;
        }

        public static UserTokens GetTokenDetails(string user)
        {
            return Tokens.FirstOrDefault(x => x.UserName.Equals(user));
        }
    }

    public class UserTokens {
        public string UserName { get; set; } = string.Empty;
        public DateTime GenerationTime { get; set; } = DateTime.MinValue;
        public string Token { get; set; } = string.Empty;
    }
}
