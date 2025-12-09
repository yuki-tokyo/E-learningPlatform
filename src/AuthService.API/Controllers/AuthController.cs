using AuthService.Application.DTO;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;

namespace AuthService.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthGrpcClient client;
        public AuthController(IAuthGrpcClient client)
        {
            this.client = client;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            try
            {
                var token = await client.Register(dto.Email, dto.Name, dto.Password);
                return Ok(token);
            }
            catch (UserAlreadyExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            try
            {
                var token = await client.Login(dto.Email, dto.Password);
                return Ok(token);
            }
            catch (InvalidCredentialsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }
    }
}
