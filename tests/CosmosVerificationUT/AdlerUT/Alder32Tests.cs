﻿using Cosmos.Security.Verification.Adler;
using Shouldly;
using Xunit;

namespace AdlerUT
{
    [Trait("AdlerUT", "Adler32Tests")]
    public class Adler32Tests
    {
        [Theory(DisplayName = "Adler-32")]
        [InlineData("Nice", "8001A203", "10000000000000011010001000000011", "10000000000000011010001000000011")]
        [InlineData("Nice Boat", "26034D0F", "100110000000110100110100001111", "00100110000000110100110100001111")]
        [InlineData("Nice Boat, James love Jane very much.", "B70C57ED", "10110111000011000101011111101101", "10110111000011000101011111101101")]
        public void Adler32Test(string data, string hex, string bin, string binWithZero)
        {
            var function = AdlerFactory.Create(AdlerTypes.Adler32);
            var hashVal = function.ComputeHash(data);
            hashVal.AsHexString(true).ShouldBe(hex);
            hashVal.AsBinString().ShouldBe(bin);
            hashVal.AsBinString(true).ShouldBe(binWithZero);
        }
    }
}