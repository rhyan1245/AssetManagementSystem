using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AMS.WebApi.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AMS.WebApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AuthenticateController : ControllerBase
  {
    private IConfiguration _config;
    private readonly UserManager<IdentityUser> _userManager;
    public AuthenticateController(UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
      _userManager = userManager;
      _config = configuration;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
    {
      if (ModelState.IsValid)
      {
        var user = await _userManager.FindByEmailAsync(userLoginDTO.Email);

        if (user == null)
        {
          return BadRequest("Invalid user");
        }
        else
        {
          var result = await _userManager.CheckPasswordAsync(user, userLoginDTO.Password);
          if (result)
          {
            var jwtToken = GenerateJwtToken(user);

            return Ok(jwtToken);
          }
          else
          {
            return BadRequest("Invalid aunthentication request");
          }
        }
      }
      else
      {
        return BadRequest("Invalid request");
      }
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegisterDTO)
    {
      if (ModelState.IsValid)
      {
        var user = await _userManager.FindByEmailAsync(userRegisterDTO.Email);

        if (user == null)
        {
          // register new user
          user = new IdentityUser
          {
            Email = userRegisterDTO.Email,
            UserName = userRegisterDTO.Email,
            EmailConfirmed = true
          };

          var result = await _userManager.CreateAsync(user, userRegisterDTO.Password);
          if (result.Succeeded)
          {
            // We need to add the user to a role
            //await _userManager.AddLoginAsync(user, );

            var jwtToken = GenerateJwtToken(user);

            return Ok(jwtToken);
          }
          else
          {
            return BadRequest("Fail to create user");
          }
        }
        else
        {
          return NoContent();
        }
      }
      else
      {
        return BadRequest("Invalid request.");
      }
    }

    private string GenerateJwtToken(IdentityUser user)
    {
      var jwtTokenHandler = new JwtSecurityTokenHandler();
      var secret = _config.GetValue<string>("JwtConfig:Secret");
      var key = Encoding.ASCII.GetBytes(secret);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[]
        {
          new Claim(JwtRegisteredClaimNames.Sub, user.Id),
          new Claim(JwtRegisteredClaimNames.Email, user.Email),
          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }),
        Expires = DateTime.UtcNow.AddHours(3),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };

      var token = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
      var jwtToken = jwtTokenHandler.WriteToken(token);

      return jwtToken;
    }
  }
}