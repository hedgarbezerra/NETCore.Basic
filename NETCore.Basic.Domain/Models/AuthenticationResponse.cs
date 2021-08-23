using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Domain.Models
{
    public class AuthenticationResponse
    {
        public bool IsAuthenticated { get; set; }
        public string Token { get; set; }
    }
}
