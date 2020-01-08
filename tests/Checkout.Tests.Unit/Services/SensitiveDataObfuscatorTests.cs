using Checkout.Repository.Helpers;
using Checkout.Services.Services;
using Shouldly;
using Xunit;

namespace Checkout.Tests.Unit.Services
{
    public class SensitiveDataObfuscatorTests
    {
        private SensitiveDataObfuscator _sut;

        public SensitiveDataObfuscatorTests()
        {
            _sut = new SensitiveDataObfuscator();
        }

        [Fact]
        public void GivenAPayment_WhenObfuscating_ShouldHideTheMiddleEightDigitsOfTheCardLongNumber()
        {
            // given +  when
            var result = _sut.ObfuscateLongCardNumber("1234567891012345");

            // then
            result.ShouldBe("1234********2345");
        }

        [Fact]
        public void GivenAPayment_WhenObfuscating_ShouldHideTheCardsCVV()
        {
            // given +  when
            var result = _sut.ObfuscateCvv("123");

            // then
            result.ShouldBe("***");
        }
    }
}
