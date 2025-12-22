using Common.DTO.Responses;
using Common.Exceptions;
using Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestsService.Application.DTO.Requests.Questions;
using TestsService.Application.DTO.Requests.Tests;
using TestsService.Domain.Interfaces.Questions;

namespace TestsService.API.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionsService service;

        public QuestionsController(IQuestionsService service)
        {
            this.service = service;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddQuestion([FromQuery] string id, [FromBody] AddQuestionRequest dto)
        {
            try
            {
                var userId = User.GetUserId();

                await service.AddQuestion(id, dto.AnswerOptions, dto.RightAnswer, dto.Content, userId);
                var response = new MessageResponse { Msg = "Вопрос добавлен!" };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Change test content", Method = "PATCH", Href = $"{baseUrl}/api/questions/change/content" });

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (QuestionException ex)
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

                await service.DeleteQuestion(id, userId);
                var response = new MessageResponse { Msg = "Вопрос удален." };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Add question", Method = "POST", Href = $"{baseUrl}/api/questions/add" });

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

        [HttpPatch("change/content")]
        public async Task<IActionResult> ChangeQuestionContent([FromQuery] string id, [FromBody] ChangeQuestionContentRequest dto)
        {
            try
            {
                var userId = User.GetUserId();

                await service.ChangeQuestionContent(id, userId, dto.Content);

                return Ok("Вопрос успешно изменен!");
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
