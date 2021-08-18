using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Domain.Models.Users
{
    public class InputUser : BaseUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
