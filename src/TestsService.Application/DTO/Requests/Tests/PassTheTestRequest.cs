using System;
using System.Collections.Generic;
using System.Text;

namespace TestsService.Application.DTO.Requests.Tests
{
    public class PassTheTestRequest
    {
        public required List<int> Answers { get; set; }
    }
}
