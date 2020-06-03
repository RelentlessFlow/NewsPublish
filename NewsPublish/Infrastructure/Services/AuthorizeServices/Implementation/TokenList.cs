using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NewsPublish.Database.Data;
using NewsPublish.Infrastructure.Helpers;
using NewsPublish.Infrastructure.Services.AdminServices.DTO;
using NewsPublish.Infrastructure.Services.AuthorizeServices.DTO;
using NewsPublish.Infrastructure.Services.AuthorizeServices.Interface;

namespace NewsPublish.Infrastructure.Services.AuthorizeServices.Implementation
{
    public class TokenList : ITokenList
    {
        public static List<UserTokenWithRight> tokenList = new List<UserTokenWithRight>();
        private readonly IConfiguration _configuration;
        
        public TokenList(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreateToken(string account)
        {
            string tokenReturn = "";
            if (account != null && "".Equals(account) != true)
            {
                // push the user’s name into a claim, so we can identify the user later on.
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, account)
                };
                //sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                //.NET Core’s JwtSecurityToken class takes on the heavy lifting and actually creates the token.
                var token = new JwtSecurityToken(
                    issuer: "jwttest",
                    audience: "jwttest",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);
                var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
                tokenReturn = tokenStr;
            }

            return tokenReturn;
        }
        
        public void delUserToken(string token)
        {
            var userTokenWithRoles = tokenList.FirstOrDefault(x => x.Token == token);
            tokenList.Remove(userTokenWithRoles);
        }
        

        public string addUserAuthe(string account, List<string> rightNames)
        {
            MyTools.ArgumentDispose(account);
            MyTools.ArgumentDispose(rightNames);
            var token = CreateToken(account);
            UserTokenWithRight ut = new UserTokenWithRight();
            ut.Account = account;
            ut.Token = token;
            ut.RightName = rightNames;
            tokenList.Add(ut);
            return token;
        }

        public bool isExistAuth(string token)
        {
            var any = tokenList.Any(x => x.Token == token);
            return any;
        }

        public UserTokenWithRight GetToken(string token)
        {
            var tokenItems = tokenList.FirstOrDefault(x => x.Token == token);
            return tokenItems;
        }

        public List<UserTokenWithRight> GetTokenLists()
        {
            return tokenList.ToList();
        }
        
    }
}