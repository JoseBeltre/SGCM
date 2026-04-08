using Xunit.Abstractions;
using BCrypt.Net;

namespace SGCM.Application.Test
{
    public class BCryptGeneratorTest
    {
        private readonly ITestOutputHelper _output;

        public BCryptGeneratorTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void GenerateHashForDoctor()
        {
            // La misma contraseña que intentas usar
            string password = "Doctor123!";
            
            // Generamos el hash usando la misma librería que el AuthService
            string hash = BCrypt.Net.BCrypt.HashPassword(password);
            
            _output.WriteLine($"Password: {password}");
            _output.WriteLine($"Hash: {hash}");
            
            // Verificamos que el hash sea válido para la misma contraseña
            bool isValid = BCrypt.Net.BCrypt.Verify(password, hash);
            Assert.True(isValid);
        }
    }
}
