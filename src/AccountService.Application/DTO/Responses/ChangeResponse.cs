using System;
using System.Collections.Generic;
using System.Text;

namespace AccountService.Application.DTO.Responses
{
    public class ChangeResponse
    {
        public required string Msg { get; set; }
        public List<ApiLink> Links { get; set; } = new();
    }
}
