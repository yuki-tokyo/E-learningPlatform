using AccountService.Application.DTO.Requests;
using AccountService.Application.DTO.Responses;
using AccountService.Domain.Exceptions;
using AccountService.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security;

namespace AccountService.API.Controllers
{
    [Authorize]
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountGrpcClient client;
        public AccountController(IAccountGrpcClient client)
        {
            this.client = client;
        }

        [HttpPatch("change/name")]
        public async Task<IActionResult> ChangeName([FromBody] ChangeNameRequest dto)
        {
            try
            {
                await client.ChangeName(dto.Name);
                var response = new ChangeResponse { Msg = "Имя успешно поменяно!" };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Change password", Method = "PATCH", Href = $"{baseUrl}/api/account/change/password" });
                response.Links.Add(new ApiLink { Rel = "Change email", Method = "PATCH", Href = $"{baseUrl}/api/account/change/email" });
                response.Links.Add(new ApiLink { Rel = "My account", Method = "GET", Href = $"{baseUrl}/api/account/me" });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpPatch("change/password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest dto)
        {
            try
            {
                await client.ChangePassword(dto.Password);
                var response = new ChangeResponse { Msg = "Пароль успешно поменян!" };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Change name", Method = "PATCH", Href = $"{baseUrl}/api/account/change/name" });
                response.Links.Add(new ApiLink { Rel = "Change email", Method = "PATCH", Href = $"{baseUrl}/api/account/change/email" });
                response.Links.Add(new ApiLink { Rel = "My account", Method = "GET", Href = $"{baseUrl}/api/account/me" });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpPatch("change/email")]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailRequest dto)
        {
            try
            {
                await client.ChangeEmail(dto.Email);
                var response = new ChangeResponse { Msg = "Код для подтверждения почты отправлен!" };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Verify changed email", Method = "POST", Href = $"{baseUrl}/api/account/change/email/verify" });
                response.Links.Add(new ApiLink { Rel = "Change name", Method = "PATCH", Href = $"{baseUrl}/api/account/change/name" });
                response.Links.Add(new ApiLink { Rel = "Change password", Method = "PATCH", Href = $"{baseUrl}/api/account/change/password" });
                response.Links.Add(new ApiLink { Rel = "My account", Method = "GET", Href = $"{baseUrl}/api/account/me" });

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

        [HttpPost("change/email/verify")]
        public async Task<IActionResult> VerifyChangedEmail([FromBody] VerifyChangedEmailRequest dto)
        {
            try
            {
                await client.VerifyChangedEmail(dto.Email, dto.Code);
                var response = new ChangeResponse { Msg = "Почта поменяна!" };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Change email", Method = "PATCH", Href = $"{baseUrl}/api/account/change/email" });
                response.Links.Add(new ApiLink { Rel = "Change name", Method = "PATCH", Href = $"{baseUrl}/api/account/change/name" });
                response.Links.Add(new ApiLink { Rel = "Change password", Method = "PATCH", Href = $"{baseUrl}/api/account/change/password" });
                response.Links.Add(new ApiLink { Rel = "My account", Method = "GET", Href = $"{baseUrl}/api/account/me" });

                return Ok(response);
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

        [HttpGet("me")]
        public async Task<IActionResult> GetMyAccount()
        {
            try
            {
                var account = await client.GetMyAccount();

                var response = new MyAccountResponse
                {
                    Name = account.Name,
                    Email = account.Email
                };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Change name", Method = "PATCH", Href = $"{baseUrl}/api/account/change/name" });
                response.Links.Add(new ApiLink { Rel = "Change password", Method = "PATCH", Href = $"{baseUrl}/api/account/change/password" });
                response.Links.Add(new ApiLink { Rel = "Change email", Method = "PATCH", Href = $"{baseUrl}/api/account/change/email" });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [AllowAnonymous]
        [HttpGet("search/user")]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            try
            {
                var user = await client.GetById(id);

                return Ok(user);
            }
            catch (UserNotFoundException ex)
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
