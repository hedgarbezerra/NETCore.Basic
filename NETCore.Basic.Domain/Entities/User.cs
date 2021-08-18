﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NETCore.Basic.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime RegistredAt { get; set; }
    }
}