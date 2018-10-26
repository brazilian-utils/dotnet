using BrazilianUtilsService.cpf.src;
using Xunit;

namespace BrazilianUtilsService.Tests.cpf.src
{
    public class GenerateCpfTest
    {
        private readonly GenerateCpf _generateCpf;

        public GenerateCpfTest()
        {
            _generateCpf = new GenerateCpf();
        }

        [Fact]
        public void ShouldReturnCpfRightLenght(){
            var result = _generateCpf.GenerateNewCpf();
            Assert.Equal(11, result.Length);
        }

        [Theory]
        [InlineData("RS")]
        public void ShouldReturnValidCpf(string value){

            ValidateCpf validateCpf = new ValidateCpf();  

            var result = validateCpf.IsValidCpf(_generateCpf.GenerateNewCpf(value));
            Assert.True(result);
        }
    }
}