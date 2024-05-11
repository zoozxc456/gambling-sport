using System;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using JWT.Builder;
using JWT.Algorithms;
using Newtonsoft.Json.Linq;
using gamblingsportmember.Models;

namespace gamblingsportmember.Helpers
{
    public class JwtHelper
    {
        private readonly JwtSecurityTokenHandler _tokenHandler;
        public JwtHelper()
        {
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public string GenerateToken(MemberModel member, int expireMinutes = 120)
        {
            //發行人
            var issuer = "microservice";
            //加密的key，拿來比對jwt-token沒有
            var signKey = "microservice@2023";
            var token = JwtBuilder.Create()
                        //所採用的雜湊演算法
                        .WithAlgorithm(new HMACSHA256Algorithm())
                        .WithSecret(signKey)
                        .AddClaim("roles", member.Role)
                        .AddClaim("jti", Guid.NewGuid().ToString())
                        .AddClaim("iss", issuer)
                        .AddClaim("sub", member.Account)
                        .AddClaim("exp", DateTimeOffset.UtcNow.AddMinutes(expireMinutes).ToUnixTimeSeconds())
                        .AddClaim("nbf", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                        .AddClaim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                        .AddClaim("idt", member.Id)
                        .Encode();
            return token;
        }

        public JwtMemberModel DecodeToken(string token)
        {
            var jwt = _tokenHandler.ReadJwtToken(token);

            var jwtModel = new JwtMemberModel
            {
                Role = jwt!.Claims!.FirstOrDefault(claim => claim.Type == "roles")!.Value,
                Account = jwt!.Claims!.FirstOrDefault(claim => claim.Type == "sub")!.Value,
                Id = Guid.Parse(jwt!.Claims!.FirstOrDefault(claim => claim.Type == "idt")!.Value)
            };
            return jwtModel;
        }
    }
}

