using Moq;
using ShipmentDiscountCalculationModule.Application.Interfaces;
using ShipmentDiscountCalculationModule.Application.Parsers;
using System;
using System.Linq;
using Xunit;

namespace ShipmentDiscountCalculationModule.Application.Tests.Parsers
{
    public class ShippingPriceDetailsParserTests
    {
        private readonly ShippingPriceDetailsParser _shippingPriceDetailsParser;
        private readonly Mock<IValidator> _validatorMock = new Mock<IValidator>();

        public ShippingPriceDetailsParserTests()
        {
            _shippingPriceDetailsParser = new ShippingPriceDetailsParser(_validatorMock.Object);
        }

        [Fact]
        public void Parse_NullArgumentShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _shippingPriceDetailsParser.Parse(null));
        }

        [Fact]
        public void Parse_InvalidStringShouldReturnInvalidObject()
        {
            var text = "INVALID STRING";
            _validatorMock.Setup(x => x.IsValid(text)).Returns(false);

            var parsedText = _shippingPriceDetailsParser.Parse(text).ElementAt(0);

            Assert.Null(parsedText.Provider);
            Assert.Null(parsedText.Provider);
            Assert.Equal(0, parsedText.Price);
        }

        [Theory]
        [InlineData("LP S 1.5")]
        [InlineData("MR M 4.9")]
        [InlineData("LP L 3")]
        [InlineData("MR S 2")]
        public void Parse_ValidStringShouldReturnValidObject(string text)
        {
            var splitText = text.Split(" ");
            _validatorMock.Setup(x => x.IsValid(text)).Returns(true);

            var parsedText = _shippingPriceDetailsParser.Parse(text).ElementAt(0);

            Assert.Equal(splitText[0], parsedText.Provider);
            Assert.Equal(splitText[1], parsedText.PackageSize);
            Assert.Equal(Decimal.Parse(splitText[2]), parsedText.Price);
        }
    }
}
