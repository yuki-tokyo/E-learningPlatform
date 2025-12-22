using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Kafka.Messages.Tests
{
    public class TestMessage
    {
        public required string UserId { get; set; } 
        public required int Points { get; set; }
    }
}
