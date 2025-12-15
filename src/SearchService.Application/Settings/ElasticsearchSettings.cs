using System;
using System.Collections.Generic;
using System.Text;

namespace SearchService.Application.Settings
{
    public class ElasticsearchSettings
    {
        public string Uri { get; set; } = "http://127.0.0.1:9200/";
    }
}
