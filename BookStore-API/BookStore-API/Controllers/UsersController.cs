using BookStore_API.Services;
using BookStore_DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILoggerService loggerService;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;

        public UsersController(ILoggerService loggerService ,SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            this.loggerService = loggerService;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.configuration = configuration;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] UserDTO userDTO)
        {
            var location = GetControllerActionName();
            try
            {
                loggerService.LogInfo($"{location}: Attempted to SignIn");
                var result = await signInManager.PasswordSignInAsync(userDTO.Username, userDTO.Password, false, false);
                if (result.Succeeded)
                {
                    var user = await userManager.FindByNameAsync(userDTO.Username);
                    var tokken =await GenerateJWT(user);
                    loggerService.LogInfo($"{location}: Successfully SignIn");
                    return Ok(new { token = tokken }) ;
                }
                loggerService.LogInfo($"{location}: Failed to SignIn");
                return Unauthorized(userDTO);
            }
            catch (Exception ex)
            {

                return InternalError($"{location}: {ex.Message} - {ex.InnerException}");
            }
          
        }

        private async Task<string> GenerateJWT(IdentityUser user)
        {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var role = await userManager.GetRolesAsync(user);
            claims.AddRange(role.Select(r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r)));
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Issuer"]));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(configuration["JWT:Issuer"], configuration["JWT:Issuer"], claims, expires: DateTime.Now.AddDays(1),signingCredentials :creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GetControllerActionName()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;
            return $"{controller} - {action}";
        }
        private ObjectResult InternalError(string message)
        {
            loggerService.LogError(message);
            return StatusCode(500, "Something went wrong please contact Administrator");
        }
    }
}
