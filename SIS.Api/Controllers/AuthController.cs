using Microsoft.AspNetCore.Mvc;
using SIS.Api.Models;
using SIS.Api.Services;

namespace SIS.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly JsonDatabase _db;

    public AuthController(JsonDatabase db) => _db = db;

    // POST /api/auth/login
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new LoginResponse { Success = false, Message = "Username and password are required." });

        var user = _db.FindUser(request.Username, request.Password);

        if (user is null)
            return Unauthorized(new LoginResponse { Success = false, Message = "Invalid username or password." });

        // Simple session token: base64 of "userId:username:timestamp"
        string tokenRaw = $"{user.Id}:{user.Username}:{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}";
        string token    = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(tokenRaw));

        return Ok(new LoginResponse
        {
            Success = true,
            Message = "Login successful.",
            Token   = token
        });
    }
}
