using ShipmentDiscountCalculationModule.Application.Interfaces;
using ShipmentDiscountCalculationModule.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipmentDiscountCalculationModule.Application.Strategies
{
    public class DiscountStrategyContext : IDiscountStrategyContext
    {
        public void ApplyDiscount(IEnumerable<Transaction> transactionHistory, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            if (transactionHistory == null || shippingPriceDetails == null)
                throw new ArgumentNullException();

            if (!transactionHistory.Any() || !shippingPriceDetails.Any())
                throw new ArgumentException();

            var lowestSDiscountStrategy = new LowestSDiscountStrategy();
            var freeLDiscountStrategy = new FreeLDiscountStrategy();

            foreach (var transaction in transactionHistory)
            {
                if (lowestSDiscountStrategy.TryApplyDiscount(transaction, transactionHistory, shippingPriceDetails))
                    continue;

                if(freeLDiscountStrategy.TryApplyDiscount(transaction, transactionHistory, shippingPriceDetails))
                    continue;
            }
        }
    }
}
