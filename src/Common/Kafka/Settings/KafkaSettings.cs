using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Kafka.Settings
{
    public class KafkaSettings
    {
        public required string BootstrapServers { get; set; }
        public required string GroupId { get; set; }
        public required KafkaTopics Topics { get; set; } 
    }
}
