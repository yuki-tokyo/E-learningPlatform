using System;
using System.Collections.Generic;
using System.Text;

namespace CoursesService.Application.DTO.Requests
{
    public class ChangeCoursePriceRequest
    {
        public required double Price { get; set; }
    }
}
