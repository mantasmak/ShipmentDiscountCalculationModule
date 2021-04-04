using ShipmentDiscountCalculationModule.Application.Models;
using ShipmentDiscountCalculationModule.Application.Strategies;
using System;
using System.Collections.Generic;
using Xunit;

namespace ShipmentDiscountCalculationModule.Application.Tests.Strategies
{
    public class FreeLDiscountStrategyTests
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
            var freeLDiscountStrategy = new FreeLDiscountStrategy();

            Assert.Throws<ArgumentNullException>(() => freeLDiscountStrategy.TryApplyDiscount(transaction, transactionHistory, shippingPriceDetails));
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
            var freeLDiscountStrategy = new FreeLDiscountStrategy();

            Assert.Throws<ArgumentException>(() => freeLDiscountStrategy.TryApplyDiscount(transaction, transactionHistory, shippingPriceDetails));
        }

        [Fact]
        public void TryApplyDiscount_ExceedanceOfMonthlyDiscountShouldReturnFalse()
        {
            var transaction = new Transaction
            {
                Date = new DateTime(2020, 5, 11),
                Size = "L",
                Provider = "LP",
                ShippingPrice = 1.5M,
                RawText = "2020-05-11 S MR"
            };

            var transactionHistory = new List<Transaction>();

            for (int i = 0; i < 2; i++)
            {
                transactionHistory.Add(new Transaction()
                {
                    Date = new DateTime(2020, 5, 11),
                    Size = "L",
                    Provider = "LP",
                    ShippingPrice = 0,
                    Discount = 5M,
                    RawText = "2020-05-11 S MR"
                });
            }

            var shippingPriceDetails = new List<ShippingPriceDetails>()
            {
                new ShippingPriceDetails()
                {
                    Provider = "LP",
                    PackageSize = "L",
                    Price = 6.9M
                },
                new ShippingPriceDetails()
                {
                    Provider = "MR",
                    PackageSize = "L",
                    Price = 4
                }
            };

            var freeLDiscountStrategy = new FreeLDiscountStrategy();

            var attempt = freeLDiscountStrategy.TryApplyDiscount(transaction, transactionHistory, shippingPriceDetails);

            Assert.False(attempt);
        }

        [Fact]
        public void TryApplyDiscount_ExceedanceOfMonthlyDiscountShouldNotAddDiscount()
        {
            var transaction = new Transaction
            {
                Date = new DateTime(2020, 5, 11),
                Size = "L",
                Provider = "LP",
                ShippingPrice = 1.5M,
                RawText = "2020-05-11 S MR"
            };

            var transactionHistory = new List<Transaction>();

            for (int i = 0; i < 2; i++)
            {
                transactionHistory.Add(new Transaction()
                {
                    Date = new DateTime(2020, 5, 11),
                    Size = "L",
                    Provider = "LP",
                    ShippingPrice = 0,
                    Discount = 5M,
                    RawText = "2020-05-11 S MR"
                });
            }

            var shippingPriceDetails = new List<ShippingPriceDetails>()
            {
                new ShippingPriceDetails()
                {
                    Provider = "LP",
                    PackageSize = "L",
                    Price = 6.9M
                },
                new ShippingPriceDetails()
                {
                    Provider = "MR",
                    PackageSize = "L",
                    Price = 4
                }
            };

            var freeLDiscountStrategy = new FreeLDiscountStrategy();

            freeLDiscountStrategy.TryApplyDiscount(transaction, transactionHistory, shippingPriceDetails);

            Assert.Equal(0, transaction.Discount);
        }

        [Fact]
        public void TryApplyDiscount_WrongTransactionSizeShouldNotAddDiscount()
        {
            var transaction = new Transaction() { Provider = "LP", Size = "S" };
            var transactionHistory = new List<Transaction>() { new Transaction() };
            var shippingPriceDetails = new List<ShippingPriceDetails>() { new ShippingPriceDetails() };
            var freeLDiscountStrategy = new FreeLDiscountStrategy();

            var attempt = freeLDiscountStrategy.TryApplyDiscount(transaction, transactionHistory, shippingPriceDetails);

            Assert.Equal(0, transaction.Discount);
            Assert.False(attempt);
        }

        [Fact]
        public void TryApplyDiscount_WrongTransactionProviderShouldNotAddDiscount()
        {
            var transaction = new Transaction() { Provider = "MR", Size = "L" };
            var transactionHistory = new List<Transaction>() { new Transaction() };
            var shippingPriceDetails = new List<ShippingPriceDetails>() { new ShippingPriceDetails() };
            var freeLDiscountStrategy = new FreeLDiscountStrategy();

            var attempt = freeLDiscountStrategy.TryApplyDiscount(transaction, transactionHistory, shippingPriceDetails);

            Assert.Equal(0, transaction.Discount);
            Assert.False(attempt);
        }

        [Fact]
        public void TryApplyDiscount_WrongAmountOfEligibleMonthsShouldNotAddDiscount()
        {
            var transaction = new Transaction() { Provider = "LP", Size = "L" };
            var transactionHistory = new List<Transaction>() { new Transaction() };
            var shippingPriceDetails = new List<ShippingPriceDetails>() { new ShippingPriceDetails() };
            var freeLDiscountStrategy = new FreeLDiscountStrategy();

            var attempt = freeLDiscountStrategy.TryApplyDiscount(transaction, transactionHistory, shippingPriceDetails);

            Assert.Equal(0, transaction.Discount);
            Assert.False(attempt);
        }

        [Fact]
        public void TryApplyDiscount_DiscountLimitNotExceedingShouldAddDiscount()
        {
            var transaction = new Transaction
            {
                Date = new DateTime(2020, 5, 11),
                Size = "L",
                Provider = "LP",
                ShippingPrice = 6.9M,
                RawText = "2020-05-11 S MR"
            };

            var transactionHistory = new List<Transaction>();

            for (int i = 0; i < 2; i++)
            {
                transactionHistory.Add(new Transaction()
                {
                    Date = new DateTime(2020, 5, 11),
                    Size = "L",
                    Provider = "LP",
                    ShippingPrice = 6.9M,
                    Discount = 0,
                    RawText = "2020-05-11 S MR"
                });
            }

            var shippingPriceDetails = new List<ShippingPriceDetails>()
            {
                new ShippingPriceDetails()
                {
                    Provider = "LP",
                    PackageSize = "L",
                    Price = 6.9M
                },
                new ShippingPriceDetails()
                {
                    Provider = "MR",
                    PackageSize = "L",
                    Price = 4
                }
            };

            var freeLDiscountStrategy = new FreeLDiscountStrategy();

            var attempt = freeLDiscountStrategy.TryApplyDiscount(transaction, transactionHistory, shippingPriceDetails);

            Assert.Equal(0M, transaction.ShippingPrice);
            Assert.Equal(6.9M, transaction.Discount);
            Assert.True(attempt);
        }

        [Fact]
        public void TryApplyDiscount_DiscountPartiallyCoveredShouldAddPartialDiscount()
        {
            var transaction = new Transaction
            {
                Date = new DateTime(2020, 5, 11),
                Size = "L",
                Provider = "LP",
                ShippingPrice = 6.9M,
                RawText = "2020-05-11 S MR"
            };

            var transactionHistory = new List<Transaction>();

            for (int i = 0; i < 2; i++)
            {
                transactionHistory.Add(new Transaction()
                {
                    Date = new DateTime(2020, 5, 11),
                    Size = "L",
                    Provider = "LP",
                    ShippingPrice = 6.9M,
                    Discount = 4,
                    RawText = "2020-05-11 S MR"
                });
            }

            var shippingPriceDetails = new List<ShippingPriceDetails>()
            {
                new ShippingPriceDetails()
                {
                    Provider = "LP",
                    PackageSize = "L",
                    Price = 6.9M
                },
                new ShippingPriceDetails()
                {
                    Provider = "MR",
                    PackageSize = "L",
                    Price = 4
                }
            };

            var freeLDiscountStrategy = new FreeLDiscountStrategy();

            var attempt = freeLDiscountStrategy.TryApplyDiscount(transaction, transactionHistory, shippingPriceDetails);

            Assert.Equal(4.9M, transaction.ShippingPrice);
            Assert.Equal(2, transaction.Discount);
            Assert.True(attempt);
        }
    }
}
