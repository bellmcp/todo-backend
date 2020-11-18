using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
// ใช้สร้าง hased password
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
// ใช้สร้าง token
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
// ใช้ติดต่อ database
using ToDo_Backend.Models;

namespace ToDo_Backend.Controllers
{
    public partial class SignIn
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class TokensController : ControllerBase
    {
        private readonly ILogger<TokensController> _logger;

        public TokensController(ILogger<TokensController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post([FromBody] SignIn s) {
            // เช็ค userid, password
            var db = new ToDoDbContext();
            var u = db.User.Find(s.UserId);                
            string p = Convert.ToBase64String(KeyDerivation.Pbkdf2(password: s.Password, salt: Convert.FromBase64String(u.Salt), prf: KeyDerivationPrf.HMACSHA1, iterationCount: 10000, numBytesRequested: 256 / 8));
            if (u.Password != p) throw new Exception("รหัสผ่านไม่ถูกต้อง");
            // สร้าง token                
			var d = new SecurityTokenDescriptor();
			d.Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, u.Id), new Claim(ClaimTypes.Role, "user") });
			d.NotBefore = DateTime.UtcNow;
			d.Expires = DateTime.UtcNow.AddHours(3);
			d.IssuedAt = DateTime.UtcNow;
			d.Issuer = "ocsc";
			d.Audience = "public";
			d.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("1234567812345678")), SecurityAlgorithms.HmacSha256Signature);	
            var h = new JwtSecurityTokenHandler();
			var token = h.CreateToken(d);
			string t = h.WriteToken(token);
            return Ok(new { token = t });
        }
    }
}
