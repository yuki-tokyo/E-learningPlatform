using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.DTO.Responses
{
    public class RegisterResponse
    {
        public required string Msg { get; set; }
        public List<ApiLink> Links { get; set; } = new();
    }
}
