using ShipmentDiscountCalculationModule.Application.Interfaces;
using ShipmentDiscountCalculationModule.Application.Models;
using System.Collections.Generic;

namespace ShipmentDiscountCalculationModule.Application.Strategies
{
    public class DiscountCalculationContext : IDiscountCalculationContext
    {
        public void ApplyDiscount(IEnumerable<Transaction> transactionHistory, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
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
