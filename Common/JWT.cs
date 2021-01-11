using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Common
{
    public class JWT
    {
        public delegate Task ClaimsIdentityDelegate(ClaimsIdentity identity);
        public delegate SecurityTokenDescriptor SecurityTokenDescriptorDelegate(SecurityTokenDescriptor descriptor, ClaimsIdentity identity, SigningCredentials credentials);
        public delegate JsonResult ExecuteDelegate(string token);
        
        /*--------------------------------------------------------*/
        
        private readonly List<Claim>        _Claims;
        private readonly SigningCredentials _SigningCredentials;
        
        /*--------------------------------------------------------*/
        
        private ClaimsIdentity          _Identity;
        private SecurityTokenDescriptor _TokenDescriptor;

        public JWT(string key)
        {
            _Claims = new List<Claim>();
            _SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature);
        }

        public JWT SetClaims(params Claim[] claims) //PayLoad's Data
        {
            _Claims.AddRange(claims);
            return this;
        }

        public JWT SetClaimsIdentity(ClaimsIdentityDelegate @delegate) //Identity's Data
        {
            _Identity = new ClaimsIdentity(_Claims);
            @delegate(_Identity);
            return this;
        }

        public JWT SetTokenDescriptor(SecurityTokenDescriptorDelegate @delegate) //Payload's Data
        {
            _TokenDescriptor = @delegate(new SecurityTokenDescriptor(), _Identity, _SigningCredentials);
            return this;
        }

        public JsonResult Execute(ExecuteDelegate @delegate) //Token
        {
            var handler = new JwtSecurityTokenHandler();
            var token   = handler.CreateToken(_TokenDescriptor);
            return @delegate(handler.WriteToken(token));
        }
    }
}