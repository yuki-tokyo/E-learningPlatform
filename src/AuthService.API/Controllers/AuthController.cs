using AuthService.Application.DTO.Requests;
using AuthService.Application.DTO.Responses;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces.gRPC;
using EmailService.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Buffers.Text;
using System.Xml;

namespace AuthService.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthGrpcClient client;
        private readonly IEmailClientForAuth emailClient;
        public AuthController(IAuthGrpcClient client, IEmailClientForAuth emailClient)
        {
            this.client = client;
            this.emailClient = emailClient;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
        {
            try
            {
                var msg = await client.Register(dto.Email, dto.Name, dto.Password);
                var response = new RegisterResponse { Msg = "Код успешно отправлен на почту!" };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "verify", Method = "POST", Href = $"{baseUrl}/api/auth/verify" });
                
                return Ok(response);
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
        public async Task<IActionResult> Login([FromBody] LoginRequest dto)
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

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] VerifyRequest dto)
        {
            try
            {
                var msg = await emailClient.VerifyEmail(dto.Email, dto.Code);
                return Ok(msg);
            }
            catch (VerificationException ex)
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
