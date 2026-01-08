using Xunit;
using FluentAssertions;
using MeuTeste.Application.Services;

namespace MeuTeste.Tests.Application
{
    public class JwtTokenServiceTests
    {
        private readonly JwtTokenService _service;
        private const string SecretKey = "ThisIsAVeryLongSecretKeyForTestingPurposes123456";

        public JwtTokenServiceTests()
        {
            _service = new JwtTokenService(SecretKey);
        }

        [Fact]
        public void GenerateToken_Should_Return_Valid_Token()
        {
            // Act
            var token = _service.GenerateToken("testuser", "User", 60);

            // Assert
            token.Should().NotBeNullOrEmpty();
            token.Split('.').Should().HaveCount(3); // JWT format: header.payload.signature
        }

        [Fact]
        public void ValidateToken_Should_Return_True_For_Valid_Token()
        {
            // Arrange
            var token = _service.GenerateToken("testuser", "User", 60);

            // Act
            var isValid = _service.ValidateToken(token);

            // Assert
            isValid.Should().BeTrue();
        }

        [Fact]
        public void ValidateToken_Should_Return_False_For_Invalid_Token()
        {
            // Act
            var isValid = _service.ValidateToken("invalid.token.here");

            // Assert
            isValid.Should().BeFalse();
        }

        [Fact]
        public void GetUsernameFromToken_Should_Return_Username()
        {
            // Arrange
            var token = _service.GenerateToken("testuser", "User", 60);

            // Act
            var username = _service.GetUsernameFromToken(token);

            // Assert
            username.Should().Be("testuser");
        }

        [Fact]
        public void GetRoleFromToken_Should_Return_Role()
        {
            // Arrange
            var token = _service.GenerateToken("testuser", "Admin", 60);

            // Act
            var role = _service.GetRoleFromToken(token);

            // Assert
            role.Should().Be("Admin");
        }
    }
}
