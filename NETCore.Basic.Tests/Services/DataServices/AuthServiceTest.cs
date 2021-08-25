using Microsoft.IdentityModel.Tokens;
using NETCore.Basic.Domain.Entities;
using NETCore.Basic.Services.Data;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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
            var ExpiresAt = jwtSecurityToken.ValidFrom.AddMinutes(15);
            var email = (string)jwtSecurityToken.Payload.GetValueOrDefault("email");
            var role = Enum.Parse(typeof(Role), (string)jwtSecurityToken.Payload.GetValueOrDefault("role"));
            var username = (string)jwtSecurityToken.Payload.GetValueOrDefault("unique_name");

            Assert.That(jwtHandler.CanReadToken(generatedToken));
            Assert.That(jwtSecurityToken.Payload.ValidTo <= ExpiresAt);
            Assert.That(jwtSecurityToken.Issuer == "NET Core API");
            Assert.AreEqual(user.Email, email);
            Assert.AreEqual(user.Role, role);
            Assert.AreEqual(user.Username, username);
            Assert.AreEqual("NET Core API", jwtSecurityToken.Issuer);
        }
    }
}
