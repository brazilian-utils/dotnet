using System;
using System.Linq;
using System.Text;
using BrazilianUtilsService.cnpj.src;
using Xunit;

namespace BrazilianUtilsService.Tests.cnpj.src
{
    public class FormatCnpjTest
    {
       private readonly FormatCnpj _formatCnpj;

       public FormatCnpjTest()
       {
            _formatCnpj = new FormatCnpj();
       }

       [Fact]
       public void ShouldReturnEmpty(){
            var result = _formatCnpj.Format("");
            Assert.Equal("", result);
       }

        [Theory]
        [InlineData("46843485000186")]
        [InlineData("46.?ABC843.485/0001-86abc")]
        [InlineData("46843485000186000000000")]
        public void shouldReturnFormattedCnpj(string value)
        {
            var result = _formatCnpj.Format(value);
            Assert.Equal("46.843.485/0001-86", result);
        }
    }
}