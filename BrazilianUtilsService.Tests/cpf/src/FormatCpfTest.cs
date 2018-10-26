using BrazilianUtilsService.cpf.src;
using Xunit;

namespace BrazilianUtilsService.Tests.cpf.src
{
    public class FormatCpfTest
    {
        private readonly FormatCpf _formatCpf;

       public FormatCpfTest()
       {
            _formatCpf = new FormatCpf();
       }

       [Fact]
       public void ShouldReturnEmpty(){
            var result = _formatCpf.Format("");
            Assert.Equal("", result);
       }

        [Theory]
        [InlineData("94389575104")]
        [InlineData("943.?ABC895.751-04abc")]
        [InlineData("94389575104000000")]
        public void shouldReturnFormattedCpf(string value)
        {
            var result = _formatCpf.Format(value);
            Assert.Equal("943.895.751-04", result);
        }
    }
}