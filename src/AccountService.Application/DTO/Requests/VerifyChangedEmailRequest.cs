using System;
using System.Collections.Generic;
using System.Text;

namespace AccountService.Application.DTO.Requests
{
    public class VerifyChangedEmailRequest
    {
        public required string Email { get; set; }
        public required string Code { get; set; }
    }
}
