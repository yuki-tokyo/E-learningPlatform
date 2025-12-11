using System;
using System.Collections.Generic;
using System.Text;

namespace AccountService.Application.DTO.Requests
{
    public class ChangeEmailRequest
    {
        public required string Email { get; set; }
    }
}
