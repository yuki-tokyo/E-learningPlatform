using System;
using System.Collections.Generic;
using System.Text;

namespace AuthService.Domain.Exceptions.Account
{
    public class MetadataException : Exception
    {
        public MetadataException(string message) : base(message) { }
    }
}
