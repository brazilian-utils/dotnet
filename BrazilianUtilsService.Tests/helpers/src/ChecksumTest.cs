using System.Collections.Generic;
using System.Linq;
using BrazilianUtilsService.helpers;
using Xunit;

namespace BrazilianUtilsService.Tests.helpers.src
{
    public class ChecksumTest
    {
        [Fact]
        public void ShouldGenerateRightChecksumWithInteger()
        {
            var result = Checksum.GenerateChecksum(new string[] {"1", "2"}, 10);

            Assert.Equal(28, result);
        }

        [Fact]
        public void ShouldGenerateRightChecksumWithList()
        {
            var result = Checksum.GenerateChecksum(new string[] {"1", "2"}, new List<int>() { 10, 9 });

            Assert.Equal(28, result);
        }
    }
}