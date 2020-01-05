using Checkout.Repository.Models;
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
            // given
            var payment = new PaymentInformation
            {
                CardDetails = new Repository.Models.CardDetails
                {
                    CardNumber = "1234567891012345",
                    CVV = "123"
                }
            };

            // when
            _sut.Obfuscate(payment);

            // then
            payment.CardDetails.CardNumber.ShouldBe("1234********2345");
        }

        [Fact]
        public void GivenAPayment_WhenObfuscating_ShouldHideTheCardsCVV()
        {
            // given
            var payment = new PaymentInformation
            {
                CardDetails = new Repository.Models.CardDetails
                {
                    CardNumber = "1234567891012345",
                    CVV = "123"
                }
            };

            // when
            _sut.Obfuscate(payment);

            // then
            payment.CardDetails.CVV.ShouldBe("***");
        }
    }
}
