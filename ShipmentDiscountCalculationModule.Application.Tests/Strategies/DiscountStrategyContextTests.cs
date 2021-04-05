using ShipmentDiscountCalculationModule.Application.Models;
using ShipmentDiscountCalculationModule.Application.Strategies;
using System;
using System.Collections.Generic;
using Xunit;

namespace ShipmentDiscountCalculationModule.Application.Tests.Strategies
{
    public class DiscountStrategyContextTests
    {
        public static IEnumerable<object[]> NullArguments =>
        new List<object[]>
        {
            new object[] { null, null },
            new object[] { null, null },
            new object[] { new List<Transaction>(), null },
            new object[] { null, new List<ShippingPriceDetails>() },
            new object[] { null, new List<ShippingPriceDetails>() },
        };

        [Theory]
        [MemberData(nameof(NullArguments))]
        public void ApplyDiscount_NullArgumentsShouldThrowArgumentNullException(
            IEnumerable<Transaction> transactionHistory, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            var discountStrategyContext = new DiscountStrategyContext();

            Assert.Throws<ArgumentNullException>(() => discountStrategyContext.ApplyDiscount(transactionHistory, shippingPriceDetails));
        }

        public static IEnumerable<object[]> EmptyArguments =>
        new List<object[]>
        {
            new object[] { new List<Transaction>(), new List<ShippingPriceDetails>() },
            new object[] { new List<Transaction>() { new Transaction() }, new List<ShippingPriceDetails>() },
            new object[] { new List<Transaction>(), new List<ShippingPriceDetails>() { new ShippingPriceDetails() } },
        };

        [Theory]
        [MemberData(nameof(EmptyArguments))]
        public void ApplyDiscount_EmptyArgumentsShouldThrowArgumentException(
            IEnumerable<Transaction> transactionHistory, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            var discountStrategyContext = new DiscountStrategyContext();

            Assert.Throws<ArgumentException>(() => discountStrategyContext.ApplyDiscount(transactionHistory, shippingPriceDetails));
        }

        [Fact]
        public void ApplyDiscount_DiscountCriteriaMetShouldAddDiscount()
        {

            var transactionHistory = new List<Transaction>();

            for (int i = 0; i < 3; i++)
            {
                transactionHistory.Add(new Transaction()
                {
                    Date = new DateTime(2020, 5, 11),
                    Size = "L",
                    Provider = "LP",
                    ShippingPrice = 6.9M,
                    Discount = 0,
                    RawText = "2020-05-11 L LP"
                });
            }

            transactionHistory.Add(new Transaction()
            {
                Date = new DateTime(2020, 5, 11),
                Size = "S",
                Provider = "MR",
                ShippingPrice = 2,
                Discount = 0,
                RawText = "2020-05-11 S MR"
            });

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
                    Provider = "LP",
                    PackageSize = "L",
                    Price = 6.9M
                },
                new ShippingPriceDetails()
                {
                    Provider = "MR",
                    PackageSize = "S",
                    Price = 2
                },
                new ShippingPriceDetails()
                {
                    Provider = "MR",
                    PackageSize = "L",
                    Price = 4
                }
            };

            var discountStrategyContext = new DiscountStrategyContext();

            discountStrategyContext.ApplyDiscount(transactionHistory, shippingPriceDetails);

            Assert.True(transactionHistory[0].Discount > 0);
            Assert.True(transactionHistory[3].Discount > 0);
        }

        [Fact]
        public void ApplyDiscount_DiscountCriteriaNotMetShouldNotAddDiscount()
        {

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
                    RawText = "2020-05-11 L LP"
                });
            }

            transactionHistory.Add(new Transaction()
            {
                Date = new DateTime(2020, 5, 11),
                Size = "S",
                Provider = "LP",
                ShippingPrice = 2,
                Discount = 0,
                RawText = "2020-05-11 S MR"
            });

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
                    Provider = "LP",
                    PackageSize = "L",
                    Price = 6.9M
                },
                new ShippingPriceDetails()
                {
                    Provider = "MR",
                    PackageSize = "S",
                    Price = 2
                },
                new ShippingPriceDetails()
                {
                    Provider = "MR",
                    PackageSize = "L",
                    Price = 4
                }
            };

            var discountStrategyContext = new DiscountStrategyContext();

            discountStrategyContext.ApplyDiscount(transactionHistory, shippingPriceDetails);

            foreach(var transaction in transactionHistory)
                Assert.True(transaction.Discount == 0);
        }
    }
}
