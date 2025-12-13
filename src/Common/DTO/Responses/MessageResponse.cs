using System;
using System.Collections.Generic;
using System.Text;

namespace Common.DTO.Responses
{
    public class MessageResponse
    {
        public required string Msg { get; set; }
        public List<ApiLink> Links { get; set; } = new();
    }
}
