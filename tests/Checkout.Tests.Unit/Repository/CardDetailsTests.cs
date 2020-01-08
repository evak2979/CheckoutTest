using Checkout.Repository.Helpers;
using Checkout.Repository.Models;
using Moq;
using Shouldly;
using Xunit;

namespace Checkout.Tests.Unit.Repository
{
    public class CardDetailsTests
    {
        private CardDetails _sut;
        private Mock<ISensitiveDataObfuscator> _mockSensitiveDataObfuscator;

        public CardDetailsTests()
        {
            _mockSensitiveDataObfuscator = new Mock<ISensitiveDataObfuscator>();
            _sut = new CardDetails("A", "B", "C", "D");
        }

        [Fact]
        public void GivenARequestToObfuscateData_WhenHavingObfuscated_CVVAndCardDetailsShouldBeMasked()
        {
            // given
            var returnedCVV = "*";
            var returnedCardNumber = "**";
            _mockSensitiveDataObfuscator.Setup(x => x.ObfuscateCvv(_sut.CVV)).Returns(returnedCVV);
            _mockSensitiveDataObfuscator.Setup(x => x.ObfuscateLongCardNumber(_sut.CardNumber)).Returns(returnedCardNumber);

            // when
            _sut.Obfuscate(_mockSensitiveDataObfuscator.Object);

            // then
            _mockSensitiveDataObfuscator.VerifyAll();
            _sut.CVV.ShouldBe(returnedCVV);
            _sut.CardNumber.ShouldBe(returnedCardNumber);
        }
    }
}
