using Common.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountService.Application.DTO.Responses
{
    public class MyAccountResponse
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public List<ApiLink> Links { get; set; } = new();
    }
}
