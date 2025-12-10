using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Application.DTO.Requests
{
    public class VerifyRequest
    {
        public required string Email { get; set; }
        public required string Code { get; set; }
    }
}
