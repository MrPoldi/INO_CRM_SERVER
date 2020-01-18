using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using INO_CRM_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace INO_CRM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MyDbContext _context;

        public AuthController(MyDbContext context)
        {
            _context = context;
        }


        [HttpPost("token")]
        public ActionResult<string> GetToken(UserModel body)
        {
            UserModel user = _context.Users.Where(u => u.Login == body.Login).Single();
            if (user.Password != body.Password)
            {
                return Unauthorized();
            }

            //Security keys
            string key = "agahkasdadluh!@asionm,cjvha!&^#a(wuhddj@nm,!#kjvlkl'l;la'v14125nljash";
            SymmetricSecurityKey symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

            //Add claims
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, user.Role.Name));

            //Create token
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "INO",
                audience: user.Login.ToString(),
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials,
                claims: claims
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}