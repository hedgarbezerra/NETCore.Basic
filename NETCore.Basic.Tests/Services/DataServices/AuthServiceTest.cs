using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Services.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace NETCore.Basic.Tests.Services.DataServices
{
    [TestFixture]
    public class AuthServiceTest
    {
        private AuthService _authService;
        private readonly string tokenKey = "c3RyaW5ndGVzdGV1bml0YXJpbw";

        [SetUp]
        public void Setup()
        {
            _authService = new AuthService(tokenKey);
        }

        [Test]
        public void GenerateToken_ValidToken()
        {
            User user = new User()
            {
                Email = "user123@gmail.com",
                Username = "user123",
                Role = Role.Administrator
            };
            var generatedToken = _authService.GenerateToken(user);
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtHandler.ReadJwtToken(generatedToken);
            var email = (string)jwtSecurityToken.Payload.GetValueOrDefault("email");
            var role = Enum.Parse(typeof(Role), (string)jwtSecurityToken.Payload.GetValueOrDefault("role"));
            var username = (string)jwtSecurityToken.Payload.GetValueOrDefault("unique_name");

            Assert.That(jwtHandler.CanReadToken(generatedToken));
            Assert.That(jwtSecurityToken.Payload.ValidTo >= DateTime.UtcNow);
            Assert.AreEqual(user.Email, email);
            Assert.AreEqual(user.Role, role);
            Assert.AreEqual(user.Username, username);
            Assert.AreEqual("NET Core API", jwtSecurityToken.Issuer);
        }

        [Test]
        [TestCase(@"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImhlZGdhcl9wZG1AaG90bWFpbG9tIiwidW5pcXVlX25hbWUiOiJqb3NlMSIsInJvbGUiOiJBZG1pbmlzdHJhdG9yIiwibmJmIjoxNjI5ODk1ODI4LCJleHAiOjE2Mjk4OTY3MjgsImlhdCI6MTYyOTg5NTgyOCwiaXNzIjoiTkVUIENvcmUgQVBJIn0.bk8hmjQ3uHZf3v85t1OivEzyS0GhteM9bceNJ_kkZWA")]
        [TestCase(@"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImhlZGdhcl9wZG1AaG90bWFpbG9tIiwidW5pcXVlX25hbWUiOiJqb3NlIiwicm9sZSI6IkNvbW1vbiIsIm5iZiI6MTYyOTg5NTg5MSwiZXhwIjoxNjI5ODk2NzkxLCJpYXQiOjE2Mjk4OTU4OTEsImlzcyI6Ik5FVCBDb3JlIEFQSSJ9.hr_yFu3Wrz6-AW8tT0up6RY5602KcG5mAV1iuhKUlb0")]
        public void GenerateToken_ExpiredToken(string token)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtHandler.ReadJwtToken(token);

            Assert.That(jwtHandler.CanReadToken(token));
            Assert.That(jwtSecurityToken.Payload.ValidTo <= DateTime.UtcNow);
        }
    }
}
