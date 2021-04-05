using ShipmentDiscountCalculationModule.Application.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ShipmentDiscountCalculationModule.Application.Tests.Validators
{
    public class ShippingPriceDetailsValidatorTests
    {
        [Fact]
        public void IsValid_NullArgumentShouldThrowArgumentNullException()
        {
            var shippingPriceDetailsValidator = new ShippingPriceDetailsValidator();

            Assert.Throws<ArgumentNullException>(() => shippingPriceDetailsValidator.IsValid(null));
        }

        [Theory]
        [InlineData("LP")]
        [InlineData("LP S")]
        [InlineData("LP S 1.5 S")]
        [InlineData("LP S 1.5 S LP")]
        public void IsValid_WrongNumberOfWordsShouldReturnFalse(string text)
        {
            var shippingPriceDetailsValidator = new ShippingPriceDetailsValidator();

            var result = shippingPriceDetailsValidator.IsValid(text);

            Assert.False(result);
        }

        [Theory]
        [InlineData("PP S 1.5")]
        [InlineData("FW M 4.9")]
        [InlineData("BC L 3")]
        [InlineData("IO S 2")]
        public void IsValid_WrongProvidersShouldReturnFalse(string text)
        {
            var shippingPriceDetailsValidator = new ShippingPriceDetailsValidator();

            var result = shippingPriceDetailsValidator.IsValid(text);

            Assert.False(result);
        }

        [Theory]
        [InlineData("LP E 1.5")]
        [InlineData("MR F 4.9")]
        [InlineData("LP V 3")]
        [InlineData("MR Z 2")]
        public void IsValid_WrongSizeShouldReturnFalse(string text)
        {
            var shippingPriceDetailsValidator = new ShippingPriceDetailsValidator();

            var result = shippingPriceDetailsValidator.IsValid(text);

            Assert.False(result);
        }

        [Theory]
        [InlineData("LP S -1.5")]
        [InlineData("MR M 0")]
        [InlineData("LP L 99999999999999999999999999999999999999999999999999999999999999")]
        [InlineData("MR S 0.000000000000000000000000000000000000000000001")]
        public void IsValid_WrongPriceShouldReturnFalse(string text)
        {
            var shippingPriceDetailsValidator = new ShippingPriceDetailsValidator();

            var result = shippingPriceDetailsValidator.IsValid(text);

            Assert.False(result);
        }

        [Theory]
        [InlineData("LP S 1.5")]
        [InlineData("MR M 4.9")]
        [InlineData("LP L 3")]
        [InlineData("MR S 2")]
        public void IsValid_CorrectStringShouldReturnTrue(string text)
        {
            var shippingPriceDetailsValidator = new ShippingPriceDetailsValidator();

            var result = shippingPriceDetailsValidator.IsValid(text);

            Assert.True(result);
        }
    }
}
