using BrazilianUtilsService.helpers;
using Xunit;

namespace BrazilianUtilsService.Tests.helpers.src
{
    public class OnlyNumbersTest
    {
        [Fact]
        public void ReturnFalseGivenValueOf1()
        {
            var result = OnlyNumbers.FetchOnlyNumbers("032.721.340-00");

            Assert.Equal("03272134000", result);
        }
    }
}