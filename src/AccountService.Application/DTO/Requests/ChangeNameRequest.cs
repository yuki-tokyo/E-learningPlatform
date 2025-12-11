using System;
using System.Collections.Generic;
using System.Text;

namespace AccountService.Application.DTO.Requests
{
    public class ChangeNameRequest
    {
        public required string Name { get; set; }
    }
}
