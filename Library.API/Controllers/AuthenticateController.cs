using Library.API.Entities;
using Library.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Library.API.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        public IConfiguration Configuration { get; }
        public UserManager<User> UserManager { get; }
        public RoleManager<Role> RoleManager { get; }

        public AuthenticateController(IConfiguration configuration, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            Configuration = configuration;
            UserManager = userManager;
            RoleManager = roleManager;
        }

        [HttpPost("token", Name = nameof(GenerateToken))]
        public IActionResult GenerateToken(LoginUser loginUser)
        {
            if (loginUser.UserName != "demouser" || loginUser.Password != "demopassword")
            {
                return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, loginUser.UserName)
            };

            var tokenConfigSection = Configuration.GetSection("Security:Token");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigSection["Key"]!));
            var signCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: tokenConfigSection["Issuer"],
                audience: tokenConfigSection["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(3),
                signingCredentials: signCredential);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                expiration = TimeZoneInfo.ConvertTimeFromUtc(jwtToken.ValidTo, TimeZoneInfo.Local)
            });
        }

        [HttpPost("register", Name = nameof(AddUserAsync))]
        public async Task<IActionResult> AddUserAsync(RegisterUser registerUser)
        {
            var user = new User
            {
                UserName = registerUser.UserName,
                Email = registerUser.Email,
                BirthDate = registerUser.BirthDate
            };

            IdentityResult result = await UserManager.CreateAsync(user, registerUser.Password!);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                ModelState.AddModelError("Error", result.Errors.FirstOrDefault()!.Description);
                return BadRequest(ModelState);
            }
        }

        [HttpPost("token2", Name = nameof(GenerateTokenAsync))]
        public async Task<IActionResult> GenerateTokenAsync(LoginUser loginUser)
        {
            var user = await UserManager.FindByEmailAsync(loginUser.UserName!);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = UserManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash!, loginUser.Password!);
            if (result != PasswordVerificationResult.Success)
            {
                return Unauthorized();
            }

            var userClaims = await UserManager.GetClaimsAsync(user);
            var userRoles = await UserManager.GetRolesAsync(user);
            foreach (var roleItem in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, roleItem));
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!)
            };

            claims.AddRange(userClaims);

            var tokenConfigSection = Configuration.GetSection("Security:Token");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfigSection["Key"]!));
            var signCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: tokenConfigSection["Issuer"],
                audience: tokenConfigSection["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(3),
                signingCredentials: signCredential);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                expiration = TimeZoneInfo.ConvertTimeFromUtc(jwtToken.ValidTo, TimeZoneInfo.Local)
            });
        }
    }
}
