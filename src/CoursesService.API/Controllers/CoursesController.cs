using Common.DTO.Responses;
using Common.Exceptions;
using Common.Extensions;
using CoursesService.Application.DTO.Requests;
using CoursesService.Domain.Exceptions;
using CoursesService.Domain.Interfaces;
using CoursesService.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoursesService.API.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICoursesService service;
        private readonly ISearchClientForCourses search;
        public CoursesController(ICoursesService service, ISearchClientForCourses search)
        {
            this.service = service;
            this.search = search;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddCourse([FromBody] AddCourseRequest dto)
        {
            try
            {
                var userId = User.GetUserId();

                await service.AddCourse(dto.Name, dto.Description, dto.Price, userId);
                var response = new MessageResponse { Msg = "Курс добавлен!" };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Change course name", Method = "PATCH", Href = $"{baseUrl}/api/courses/change/name" });
                response.Links.Add(new ApiLink { Rel = "Change course description", Method = "PATCH", Href = $"{baseUrl}/api/courses/change/description" });
                response.Links.Add(new ApiLink { Rel = "Change course price", Method = "PATCH", Href = $"{baseUrl}/api/courses/change/price" });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCourse([FromQuery] string id)
        {
            try
            {
                var userId = User.GetUserId();

                await service.DeleteCourse(id, userId);
                var response = new MessageResponse { Msg = "Курс удален!" };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Add course", Method = "POST", Href = $"{baseUrl}/api/courses/add" });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpPatch("change/name")]
        public async Task<IActionResult> ChangeName([FromQuery] string id, [FromBody] ChangeCourseNameRequest dto)
        {
            try
            {
                var userId = User.GetUserId();

                await service.UpdateCourseName(id, dto.Name, userId);
                var response = new MessageResponse { Msg = "Название курса изменено!" };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Change course description", Method = "PATCH", Href = $"{baseUrl}/api/courses/change/description" });
                response.Links.Add(new ApiLink { Rel = "Change course price", Method = "PATCH", Href = $"{baseUrl}/api/courses/change/price" });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpPatch("change/description")]
        public async Task<IActionResult> ChangeDescription([FromQuery] string id, [FromBody] ChangeCourseDescRequest dto)
        {
            try
            {
                var userId = User.GetUserId();

                await service.UpdateCourseDescription(id, dto.Description, userId);
                var response = new MessageResponse { Msg = "Описание курса изменено!" };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Change course name", Method = "PATCH", Href = $"{baseUrl}/api/courses/change/name" });
                response.Links.Add(new ApiLink { Rel = "Change course price", Method = "PATCH", Href = $"{baseUrl}/api/courses/change/price" });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpPatch("change/price")]
        public async Task<IActionResult> ChangePrice([FromQuery] string id, [FromBody] ChangeCoursePriceRequest dto)
        {
            try
            {
                var userId = User.GetUserId();

                await service.UpdateCoursePrice(id, dto.Price, userId);
                var response = new MessageResponse { Msg = "Стоимость курса изменена!" };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Change course name", Method = "PATCH", Href = $"{baseUrl}/api/courses/change/name" });
                response.Links.Add(new ApiLink { Rel = "Change course description", Method = "PATCH", Href = $"{baseUrl}/api/courses/change/description" });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpPost("buy")]
        public async Task<IActionResult> BuyCourse([FromQuery] string id)
        {
            try
            {
                var userId = User.GetUserId();

                await service.BuyCourse(id, userId);
                var response = new MessageResponse { Msg = "Курс куплен!" };

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                response.Links.Add(new ApiLink { Rel = "Get courses I bought", Method = "GET", Href = $"{baseUrl}/api/courses/my/library" });
                response.Links.Add(new ApiLink { Rel = "Get courses I posted", Method = "GET", Href = $"{baseUrl}/api/courses/my/courses" });

                return Ok(response);
            }
            catch (NotEnoughMoneyException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (CoursePurchaseException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpGet("my/library")]
        public async Task<IActionResult> CoursesIBought()
        {
            try
            {
                var userId = User.GetUserId();

                var courses = await service.GetCoursesIBought(userId);

                return Ok(courses);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpGet("my/courses")]
        public async Task<IActionResult> CoursesIPosted()
        {
            try
            {
                var userId = User.GetUserId();

                var courses = await service.GetCoursesIPosted(userId);

                return Ok(courses);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCourses([FromQuery] string q)
        {
            try
            {
                var courses = await search.SearchCourses(q);

                return Ok(courses);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex}");
            }
        }
    }
}
