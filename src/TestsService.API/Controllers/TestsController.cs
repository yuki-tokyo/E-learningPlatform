using Common.DTO.Responses;
using Common.Exceptions;
using Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestsService.Application.DTO.Requests.Tests;
using TestsService.Domain.Interfaces.Tests;

namespace TestsService.API.Controllers
{
    [Route("api/tests")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly ITestsService service;

        public TestsController(ITestsService service)
        {
            this.service = service;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddTest([FromBody] AddTestRequest dto)
        {
            try
            {
                var userId = User.GetUserId();

                await service.AddTest(dto.LectureId, dto.Name, userId);
                var response = new MessageResponse { Msg = "Тест добавлен!" };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Change test name", Method = "PATCH", Href = $"{baseUrl}/api/tests/change/name" });

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (TestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpPost("run")]
        public async Task<IActionResult> PassTheTest([FromQuery] string id, [FromBody] PassTheTestRequest dto)
        {
            try
            {
                var userId = User.GetUserId();

                var isTestPassed = await service.PassTheTest(id, dto.Answers, userId);

                if (!isTestPassed)
                {
                    return BadRequest("Тест не пройден :(");
                }

                return Ok("Тест успешно пройден!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (TestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteTest([FromQuery] string id)
        {
            try
            {
                var userId = User.GetUserId();

                await service.DeleteTest(id, userId);
                var response = new MessageResponse { Msg = "Тест удален :(" };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Add test", Method = "POST", Href = $"{baseUrl}/api/tests/add" });

                return Ok(response);
            }
            catch (TestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpPatch("change/name")]
        public async Task<IActionResult> ChangeTestName([FromQuery] string id, [FromBody] ChangeTestNameRequest dto)
        {
            try
            {
                var userId = User.GetUserId();

                await service.ChangeTestName(id, userId, dto.Name);

                return Ok("Название теста успешно изменено!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (TestException ex)
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
