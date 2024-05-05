using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PollingServer.Models;
using PollingServer.Models.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PollingServer.Controllers.Authorization
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : Controller
    {
        protected JsonWebTokenSettings tokenSettings { get;  set; }
        protected DatabaseContext databaseContext { get; set; }

        public AuthorizationController(IConfiguration configuration, DatabaseContext databaseContext)
        {
            this.tokenSettings = new JsonWebTokenSettings
            {
                Issuer = configuration["TokenSettings:ISSUER"],
                Audience = configuration["TokenSettings:AUDIENCE"],
                Key = configuration["TokenSettings:KEY"]
            };
            this.databaseContext = databaseContext;
        }

        [HttpPost]
        [Route("authorize")]
        public IActionResult Authorize([FromBody] AuthorizationRequestModel model )
        {
            // Verify model field attributes
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Verify user exists in database
            try
            {
                var user = databaseContext.Users.Single((user) => user.Nickname == model.Nickname);
                if( user.Password != model.Password )
                    return BadRequest(BadResponseFactory.CreateErrorResponse(
                        "custom-error", 
                        "Authorization error", 
                        new[] { 
                            new { error = "Wrong login or password" } 
                        })
                    );
                var jwt = CreateJsonWebToken(user);
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                return Json(encodedJwt);
            } catch(InvalidOperationException exception)
            {
                return BadRequest(BadResponseFactory.CreateErrorResponse(
                    "custom-error",
                    "Authorization error",
                    new[] {
                        new { error = "Wrong login or password" }
                    })
                );
            }
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Registrate([FromBody] RegistrationRequestModel model)
        {
            // Verify model field attributes
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Verify user already exists in database
            var existingUsers = databaseContext.Users.Where((user) => user.Nickname == model.Nickname || user.Email == model.Email);
            if( existingUsers.Count() > 0)
            {
                return BadRequest(BadResponseFactory.CreateErrorResponse(
                        "custom-error",
                        "Authorization error",
                        new[] {
                            new { error = "Username or email is already used" }
                        })
                    );
            }
            // Create user entity and store it to database context
            var createdUser = new User
            {
                Nickname = model.Nickname,
                Password = model.Password,
                Email = model.Email
            };
            databaseContext.Users.Add(createdUser);
            databaseContext.SaveChanges();
            var jwt = CreateJsonWebToken(createdUser);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Json(encodedJwt);
        }

        [NonAction]
        protected JwtSecurityToken CreateJsonWebToken(User user)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Nickname),
                new Claim(ClaimTypes.Email, user.Email),
            };
            // Create JWT token
            var jwt = new JwtSecurityToken(
                issuer: tokenSettings.Issuer,
                audience: tokenSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                signingCredentials: new SigningCredentials(tokenSettings.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
            return jwt;
        }
    }
}
