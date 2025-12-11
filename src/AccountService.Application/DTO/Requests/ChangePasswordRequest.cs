using System;
using System.Collections.Generic;
using System.Text;

namespace AccountService.Application.DTO.Requests
{
    public class ChangePasswordRequest
    {
        public required string Password { get; set; }
    }
}
