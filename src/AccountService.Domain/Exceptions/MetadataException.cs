using System;
using System.Collections.Generic;
using System.Text;

namespace AccountService.Domain.Exceptions
{
    public class MetadataException : Exception
    {
        public MetadataException(string message) : base(message) { }
    }
}
