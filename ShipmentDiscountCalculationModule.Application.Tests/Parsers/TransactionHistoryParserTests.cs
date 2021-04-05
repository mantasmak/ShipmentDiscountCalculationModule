using Moq;
using ShipmentDiscountCalculationModule.Application.Interfaces;
using ShipmentDiscountCalculationModule.Application.Parsers;
using System;
using System.Linq;
using Xunit;

namespace ShipmentDiscountCalculationModule.Application.Tests.Parsers
{
    public class TransactionHistoryParserTests
    {
        private readonly TransactionHistoryParser _transactionHistoryParser;
        private readonly Mock<IValidator> _validatorMock = new Mock<IValidator>();

        public TransactionHistoryParserTests()
        {
            _transactionHistoryParser = new TransactionHistoryParser(_validatorMock.Object);
        }

        [Fact]
        public void Parse_NullArgumentShouldThrowArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _transactionHistoryParser.Parse(null));
        }

        [Fact]
        public void Parse_InvalidStringShouldReturnInvalidObject()
        {
            var text = "INVALID STRING";
            _validatorMock.Setup(x => x.IsValid(text)).Returns(false);

            var parsedText = _transactionHistoryParser.Parse(text).ElementAt(0);

            Assert.True(parsedText.WrongTransactionFormat);
            Assert.Equal(text, parsedText.RawText);
        }

        [Theory]
        [InlineData("2021-01-12 L LP")]
        [InlineData("2016-09-12 S LP")]
        [InlineData("2002-04-17 M MR")]
        [InlineData("2012-11-11 S MR")]
        public void Parse_ValidStringShouldReturnValidObject(string text)
        {
            var splitText = text.Split(" ");
            _validatorMock.Setup(x => x.IsValid(text)).Returns(true);

            var parsedText = _transactionHistoryParser.Parse(text).ElementAt(0);

            Assert.Equal(splitText[0], parsedText.Date.ToShortDateString());
            Assert.Equal(splitText[1], parsedText.Size);
            Assert.Equal(splitText[2], parsedText.Provider);
            Assert.False(parsedText.WrongTransactionFormat);
            Assert.Equal(text, parsedText.RawText);
        }
    }
}
