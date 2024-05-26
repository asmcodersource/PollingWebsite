using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PollingServer.Controllers.Authorization.DTOs;
using PollingServer.Controllers.Poll;
using PollingServer.Models;
using PollingServer.Models.Poll.Question;
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
        [Route("tokenvalidation")]
        [ProducesResponseType(typeof(TokenValidationResponseDTO), 200)]
        [ProducesResponseType(401)]
        public IActionResult TokenValidation()
        {
            var userId = HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            Models.User.User? user = databaseContext.Users.FirstOrDefault((user) => user.Id == Convert.ToInt32(userId));
            var response = new TokenValidationResponseDTO(Convert.ToInt32(userId), user!.Nickname);
            if (userId is not null)
                return Json(response);
            else
                return base.StatusCode(StatusCodes.Status401Unauthorized);
        }

        [HttpPost]
        [Route("authorize")]
        public IActionResult Authorize([FromBody] AuthorizationRequestDTO model )
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
                        new []{ "Wrong login or password" }
                        )
                    );
                var jwt = CreateJsonWebToken(user);
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                return Json(new { token = encodedJwt });
            } catch(InvalidOperationException exception)
            {
                return BadRequest(BadResponseFactory.CreateErrorResponse(
                    "custom-error",
                    "Authorization error",
                    new[] { 
                        "Wrong login or password"
                    })
                );
            }
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Registrate([FromBody] RegistrationRequestDTO model)
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
                        new[] { "Username or email is already used" }
                        )
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
            return Json(new { token = encodedJwt });
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
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(360)),
                signingCredentials: new SigningCredentials(tokenSettings.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );
            return jwt;
        }
    }
}
