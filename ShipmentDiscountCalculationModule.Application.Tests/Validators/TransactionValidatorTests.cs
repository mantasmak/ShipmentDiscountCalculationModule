using ShipmentDiscountCalculationModule.Application.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ShipmentDiscountCalculationModule.Application.Tests.Validators
{
    public class TransactionValidatorTests
    {
        [Fact]
        public void IsValid_NullArgumentShouldThrowArgumentNullException()
        {
            var transactionValidator = new TransactionValidator();

            Assert.Throws<ArgumentNullException>(() => transactionValidator.IsValid(null));
        }

        [Theory]
        [InlineData("2012-01-12")]
        [InlineData("2012-01-12 S")]
        [InlineData("2012-01-12 S MR MR")]
        [InlineData("2012-01-12 S MR LR WR")]
        public void IsValid_WrongNumberOfWordsShouldReturnFalse(string text)
        {
            var transactionValidator = new TransactionValidator();

            var result = transactionValidator.IsValid(text);

            Assert.False(result);
        }

        [Theory]
        [InlineData("20120112 S LP")]
        [InlineData("2012/03/12 M LP")]
        [InlineData("2012_01_17 L MR")]
        [InlineData("11-01-2012 M MR")]
        public void IsValid_WrongDateFormatShouldReturnFalse(string text)
        {
            var transactionValidator = new TransactionValidator();

            var result = transactionValidator.IsValid(text);

            Assert.False(result);
        }

        [Theory]
        [InlineData("2012-01-12 S UP")]
        [InlineData("2012-03-12 L JK")]
        [InlineData("2012-01-17 S CX")]
        [InlineData("2012-01-11 L WR")]
        public void IsValid_WrongProviderShouldReturnFalse(string text)
        {
            var transactionValidator = new TransactionValidator();

            var result = transactionValidator.IsValid(text);

            Assert.False(result);
        }

        [Theory]
        [InlineData("2012-01-12 Z LP")]
        [InlineData("2012-03-12 Q LP")]
        [InlineData("2012-01-17 F MR")]
        [InlineData("2012-01-11 O MR")]
        public void IsValid_WrongSizeShouldReturnFalse(string text)
        {
            var transactionValidator = new TransactionValidator();

            var result = transactionValidator.IsValid(text);

            Assert.False(result);
        }

        [Theory]
        [InlineData("2021-01-12 L LP")]
        [InlineData("2016-09-12 S LP")]
        [InlineData("2002-04-17 M MR")]
        [InlineData("2012-11-11 S MR")]
        public void IsValid_CorrectStringShouldReturnTrue(string text)
        {
            var transactionValidator = new TransactionValidator();

            var result = transactionValidator.IsValid(text);

            Assert.True(result);
        }
    }
}
