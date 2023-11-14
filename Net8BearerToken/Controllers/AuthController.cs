using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Net8BearerToken.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{

    [HttpGet("/login")]
    public IActionResult Login(string username, string password)
    {

        if (IsValidUser(username, password))
        {
            var claimsPrincipal = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new[] { new Claim(ClaimTypes.Name, username) },
                    BearerTokenDefaults.AuthenticationScheme
                )
            );

            return SignIn(claimsPrincipal);
        }
        return Unauthorized("Credenciais inválidas");
    }

    private bool IsValidUser(string username, string password)
    {
        return username == "macoratti" && password == "123456";
    }

    [HttpGet("/user")]
    [Authorize]
    public IActionResult GetUser()
    {
        var user = User;
        if (user?.Identity?.IsAuthenticated ?? false)
        {
            return Ok($"Bem-Vindo {user.Identity.Name}!");
        }

        return Unauthorized();
    }
}
