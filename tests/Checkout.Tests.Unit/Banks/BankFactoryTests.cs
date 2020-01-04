using System;
using Checkout.Services.Banks;
using Checkout.Services.Banks.DummyBank;
using Shouldly;
using Xunit;

namespace Checkout.Tests.Unit.Services
{
    public class BankFactoryTests
    {
        private BankFactory _sut;

        public BankFactoryTests()
        {
            _sut = new BankFactory();
        }

        [Fact]
        public void GivenAVendorMerchantId_WhenCreatingABank_ShouldReturnDummyBank()
        {
            // given + when
            var response = _sut.Create(Guid.NewGuid());

            // then
            response.ShouldBeOfType<DummyBank>();
        }
    }
}
