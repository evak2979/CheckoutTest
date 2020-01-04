using Checkout.Services.Banks;
using Checkout.Services.Banks.DummyBank;
using Shouldly;
using Xunit;

namespace Checkout.Tests.Unit.Banks
{
    public class DummyBankTests
    {
        private DummyBank _sut;

        public DummyBankTests()
        {
            _sut = new DummyBank();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void GivenAPayment_WhenAmountLessThanOrEqualToZero_ShouldReturnUnsuccesfullResponse(decimal amount)
        {
            // given + when
            var response = _sut.ProcessPayment(new BankPaymentRequest
            {
                Amount = amount
            });

            response.Message.ShouldBe("Unsuccessful Payment - Random reason");
        }
    }
}
