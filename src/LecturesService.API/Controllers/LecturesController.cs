using Common.DTO.Responses;
using Common.Exceptions;
using Common.Extensions;
using LecturesService.Application.DTO.Requests;
using LecturesService.Domain.Exceptions;
using LecturesService.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LecturesService.API.Controllers
{
    [Route("api/lectures")]
    [ApiController]
    public class LecturesController : ControllerBase
    {
        private readonly ILecturesService service;
        public LecturesController(ILecturesService service)
        {
            this.service = service;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddLecture([FromBody] AddLectureRequest dto)
        {
            try
            {
                var userId = User.GetUserId();

                await service.AddLecture(dto.CourseId, userId, dto.Name, dto.Content);
                var response = new MessageResponse { Msg = "Лекция добавлена!" };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Change lecture name", Method = "PATCH", Href = $"{baseUrl}/api/lectures/change/name" });
                response.Links.Add(new ApiLink { Rel = "Change lecture content", Method = "PATCH", Href = $"{baseUrl}/api/lectures/change/content" });

                return Ok(response);
            }
            catch (LectureException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (CourseNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpPatch("change/name")]
        public async Task<IActionResult> ChangeLectureName([FromQuery] string id, [FromBody] ChangeLectureNameRequest dto)
        {
            try
            {
                var userId = User.GetUserId();

                await service.ChangeLectureName(id, userId, dto.Name);
                var response = new MessageResponse { Msg = "Название лекции изменено!" };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Change lecture content", Method = "PATCH", Href = $"{baseUrl}/api/lectures/change/content" });

                return Ok(response);
            }
            catch (LectureException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpPatch("change/content")]
        public async Task<IActionResult> ChangeLectureContent([FromQuery] string id, [FromBody] ChangeLectureContentRequest dto)
        {
            try
            {
                var userId = User.GetUserId();

                await service.ChangeLectureContent(id, userId, dto.Content);
                var response = new MessageResponse { Msg = "Содержание лекции изменено!" };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Change lecture name", Method = "PATCH", Href = $"{baseUrl}/api/lectures/change/name" });

                return Ok(response);
            }
            catch (LectureException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteLecture([FromQuery] string id)
        {
            try
            {
                var userId = User.GetUserId();

                await service.DeleteLecture(id, userId);
                var response = new MessageResponse { Msg = "Лекция удалена :(" };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Add lecture", Method = "POST", Href = $"{baseUrl}/api/lectures/add" });

                return Ok(response);
            }
            catch (LectureException ex)
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
