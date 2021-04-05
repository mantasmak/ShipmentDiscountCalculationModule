using Moq;
using ShipmentDiscountCalculationModule.Application.Interfaces;
using ShipmentDiscountCalculationModule.Application.Models;
using ShipmentDiscountCalculationModule.Application.Services;
using System;
using Xunit;

namespace ShipmentDiscountCalculationModule.Application.Tests.Services
{
    public class ShippmentPriceCalculationServiceTests
    {
        private readonly ShippmentPriceCalculationService _shippmentPriceCalculationService;
        private readonly Mock<IParser<Transaction>> _transactionHistoryParserMock = new Mock<IParser<Transaction>>();
        private readonly Mock<IParser<ShippingPriceDetails>> _shippingPriceDetailsParserMock = new Mock<IParser<ShippingPriceDetails>>();
        private readonly Mock<IDiscountStrategyContext> _discountStrategyContext = new Mock<IDiscountStrategyContext>();

        public ShippmentPriceCalculationServiceTests()
        {
            _shippmentPriceCalculationService = new ShippmentPriceCalculationService(_transactionHistoryParserMock.Object, _shippingPriceDetailsParserMock.Object, _discountStrategyContext.Object);
        }

        [Theory]
        [InlineData(null,null)]
        [InlineData("", null)]
        [InlineData(null , "")]
        public void AddDiscount_NullArgumentShouldThrowArgumentNullException(string transactionHistory, string shippingPriceDetails)
        {
            Assert.Throws<ArgumentNullException>(() => _shippmentPriceCalculationService.AddDiscount(transactionHistory, shippingPriceDetails));
        }
    }
}
