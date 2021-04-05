using System;
using System.Collections.Generic;
using Xunit;
using ShipmentDiscountCalculationModule.Application.Models;
using ShipmentDiscountCalculationModule.Application.Strategies;

namespace ShipmentDiscountCalculationModule.Application.Tests.Strategies
{
    public class LowestSDiscountStrategyTests
    {
        public static IEnumerable<object[]> NullArguments =>
        new List<object[]>
        {
            new object[] { null, null, null },
            new object[] { new Transaction(), null, null },
            new object[] { new Transaction(), new List<Transaction>(), null },
            new object[] { new Transaction(), null, new List<ShippingPriceDetails>() },
            new object[] { null, null, new List<ShippingPriceDetails>() },
        };

        [Theory]
        [MemberData(nameof(NullArguments))]
        public void TryApplyDiscount_NullArgumentsShouldThrowArgumentNullException(
            Transaction transaction, IEnumerable<Transaction> transactionHistory, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            var lowestSDiscountStrategy = new LowestSDiscountStrategy();

            Assert.Throws<ArgumentNullException>(() => lowestSDiscountStrategy.TryApplyDiscount(transaction, transactionHistory, shippingPriceDetails));
        }

        public static IEnumerable<object[]> EmptyArguments =>
        new List<object[]>
        {
            new object[] { new Transaction() { Provider = "LP" }, new List<Transaction>(), new List<ShippingPriceDetails>() },
            new object[] { new Transaction(), new List<Transaction>() { new Transaction() }, new List<ShippingPriceDetails>() },
            new object[] { new Transaction(), new List<Transaction>(), new List<ShippingPriceDetails>() { new ShippingPriceDetails() } },
        };

        [Theory]
        [MemberData(nameof(EmptyArguments))]
        public void TryApplyDiscount_EmptyArgumentsShouldThrowArgumentException(
            Transaction transaction, IEnumerable<Transaction> transactionHistory, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            var lowestSDiscountStrategy = new LowestSDiscountStrategy();

            Assert.Throws<ArgumentException>(() => lowestSDiscountStrategy.TryApplyDiscount(transaction, transactionHistory, shippingPriceDetails));
        }

        [Fact]
        public void TryApplyDiscount_ExceedanceOfMonthlyDiscountShouldNotAddDiscount()
        {
            var transaction = new Transaction
            {
                Date = new DateTime(2020, 5, 11),
                Size = "S",
                Provider = "MR",
                ShippingPrice = 1.5M,
                RawText = "2020-05-11 S MR"
            };

            var transactionHistory = new List<Transaction>();

            for (int i = 0; i < 20; i++)
            {
                transactionHistory.Add(new Transaction()
                {
                    Date = new DateTime(2020, 5, 11),
                    Size = "S",
                    Provider = "MR",
                    ShippingPrice = 1.5M,
                    Discount = 0.5M,
                    RawText = "2020-05-11 S MR"
                });
            }

            var shippingPriceDetails = new List<ShippingPriceDetails>()
            {
                new ShippingPriceDetails()
                {
                    Provider = "LP",
                    PackageSize = "S",
                    Price = 1.5M
                },
                new ShippingPriceDetails()
                {
                    Provider = "MR",
                    PackageSize = "S",
                    Price = 2
                }
            };

            var lowestSDiscountStrategy = new LowestSDiscountStrategy();

            var attempt = lowestSDiscountStrategy.TryApplyDiscount(transaction, transactionHistory, shippingPriceDetails);

            Assert.Equal(1.5M, transaction.ShippingPrice);
            Assert.Equal(0, transaction.Discount);
            Assert.False(attempt);
        }

        [Fact]
        public void TryApplyDiscount_WrongTransactionSizeShouldNotAddDiscount()
        {
            var transaction = new Transaction() { Size = "L" };
            var transactionHistory = new List<Transaction>() { new Transaction() };
            var shippingPriceDetails = new List<ShippingPriceDetails>() { new ShippingPriceDetails() };
            var lowestSDiscountStrategy = new LowestSDiscountStrategy();

            var attempt = lowestSDiscountStrategy.TryApplyDiscount(transaction, transactionHistory, shippingPriceDetails);

            Assert.Equal(0, transaction.ShippingPrice);
            Assert.Equal(0, transaction.Discount);
            Assert.False(attempt);
        }

        [Fact]
        public void TryApplyDiscount_TransactionPriceIsLowestShouldNotAddDiscount()
        {
            var transaction = new Transaction
            {
                Date = new DateTime(2020, 5, 11),
                Size = "S",
                Provider = "LP",
                ShippingPrice = 1.5M,
                RawText = "2020-05-11 S LP"
            };

            var transactionHistory = new List<Transaction>()
            {
                new Transaction()
                {
                    Date = new DateTime(2020, 5, 11),
                    Size = "S",
                    Provider = "MR",
                    ShippingPrice = 1.5M,
                    Discount = 0.5M,
                    RawText = "2020-05-11 S MR"
                }
            };

            var shippingPriceDetails = new List<ShippingPriceDetails>()
            {
                new ShippingPriceDetails()
                {
                    Provider = "LP",
                    PackageSize = "S",
                    Price = 1.5M
                },
                new ShippingPriceDetails()
                {
                    Provider = "MR",
                    PackageSize = "S",
                    Price = 2
                }
            };

            var lowestSDiscountStrategy = new LowestSDiscountStrategy();

            var attempt = lowestSDiscountStrategy.TryApplyDiscount(transaction, transactionHistory, shippingPriceDetails);

            Assert.Equal(1.5M, transaction.ShippingPrice);
            Assert.Equal(0, transaction.Discount);
            Assert.False(attempt);
        }

        [Fact]
        public void TryApplyDiscount_DiscountLimitNotExceedingShouldAddDiscount()
        {
            var transaction = new Transaction
            {
                Date = new DateTime(2020, 5, 11),
                Size = "S",
                Provider = "MR",
                ShippingPrice = 2M,
                RawText = "2020-05-11 S MR"
            };

            var transactionHistory = new List<Transaction>()
            {
                new Transaction()
            };

            var shippingPriceDetails = new List<ShippingPriceDetails>()
            {
                new ShippingPriceDetails()
                {
                    Provider = "LP",
                    PackageSize = "S",
                    Price = 1.5M
                },
                new ShippingPriceDetails()
                {
                    Provider = "MR",
                    PackageSize = "S",
                    Price = 2
                }
            };

            var lowestSDiscountStrategy = new LowestSDiscountStrategy();

            var attempt = lowestSDiscountStrategy.TryApplyDiscount(transaction, transactionHistory, shippingPriceDetails);

            Assert.Equal(1.5M, transaction.ShippingPrice);
            Assert.Equal(0.5M, transaction.Discount);
            Assert.True(attempt);
        }

        [Fact]
        public void TryApplyDiscount_DiscountPartiallyCoveredShouldAddPartialDiscount()
        {
            var transaction = new Transaction
            {
                Date = new DateTime(2020, 5, 11),
                Size = "S",
                Provider = "MR",
                ShippingPrice = 2M,
                RawText = "2020-05-11 S MR"
            };

            var transactionHistory = new List<Transaction>()
            {
                new Transaction
                {
                    Date = new DateTime(2020, 5, 11),
                    Size = "L",
                    Provider = "MR",
                    ShippingPrice = 2M,
                    Discount = 9.9M,
                    RawText = "2020-05-11 S MR"
                }
            };

            var shippingPriceDetails = new List<ShippingPriceDetails>()
            {
                new ShippingPriceDetails()
                {
                    Provider = "LP",
                    PackageSize = "S",
                    Price = 1.5M
                },
                new ShippingPriceDetails()
                {
                    Provider = "MR",
                    PackageSize = "S",
                    Price = 2
                }
            };

            var lowestSDiscountStrategy = new LowestSDiscountStrategy();

            var attempt = lowestSDiscountStrategy.TryApplyDiscount(transaction, transactionHistory, shippingPriceDetails);

            Assert.Equal(1.9M, transaction.ShippingPrice);
            Assert.Equal(0.1M, transaction.Discount);
            Assert.True(attempt);
        }
    }
}
