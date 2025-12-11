using System;
using System.Collections.Generic;
using System.Text;

namespace AccountService.Application.DTO.Responses
{
    public class ApiLink
    {
        public required string Rel { get; set; }
        public required string Href { get; set; }
        public required string Method { get; set; }
    }
}
