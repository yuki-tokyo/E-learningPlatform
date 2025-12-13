using AuthService.Application.DTO.Requests;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Exceptions.Email;
using AuthService.Domain.Interfaces;
using AuthService.Domain.Interfaces.gRPC;
using Common.DTO.Responses;
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
        private readonly IAuthService service;
        private readonly IEmailClientForAuth emailClient;
        public AuthController(IAuthService service, IEmailClientForAuth emailClient)
        {
            this.service = service;
            this.emailClient = emailClient;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
        {
            try
            {
                var msg = await service.Register(dto.Name, dto.Email, dto.Password);
                var response = new MessageResponse { Msg = "Код успешно отправлен на почту!" };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Verify", Method = "POST", Href = $"{baseUrl}/api/auth/verify" });
                
                return Ok(response);
            }
            catch (UserAlreadyExistsException ex)
            {
                return BadRequest(ex.Message);
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest dto)
        {
            try
            {
                var token = await service.Login(dto.Email, dto.Password);
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
