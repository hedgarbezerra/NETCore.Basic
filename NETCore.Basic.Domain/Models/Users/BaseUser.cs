﻿using NETCore.Basic.Domain.Entities;
using System;

namespace NETCore.Basic.Domain.Models.Users
{
    public abstract class BaseUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime RegistredAt { get; set; }
        public Role Role { get; set; }
    }
}
