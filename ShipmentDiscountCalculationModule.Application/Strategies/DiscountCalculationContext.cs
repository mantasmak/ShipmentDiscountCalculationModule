using ShipmentDiscountCalculationModule.Application.Interfaces;
using ShipmentDiscountCalculationModule.Application.Models;
using System.Collections.Generic;

namespace ShipmentDiscountCalculationModule.Application.Strategies
{
    public class DiscountCalculationContext : IDiscountCalculationContext
    {
        public IEnumerable<Transaction> ApplyDiscount(IEnumerable<Transaction> transactionHistory, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            var transactionsWithDiscounts = new List<Transaction>();

            foreach(var newTransaction in transactionHistory)
            {
                if (newTransaction.Size.Equals("S"))
                {
                    var lowestSPriceStrategy = new LowestSDiscountStrategy();
                    transactionsWithDiscounts.Add(lowestSPriceStrategy.CalculateDiscount(transactionsWithDiscounts, newTransaction, shippingPriceDetails));
                    continue;
                }

                if(newTransaction.Size.Equals("L") && newTransaction.Provider.Equals("LP"))
                {
                    var freeLStrategy = new FreeLDiscountStrategy();
                    transactionsWithDiscounts.Add(freeLStrategy.CalculateDiscount(transactionsWithDiscounts, newTransaction, shippingPriceDetails));
                    continue;
                }

                transactionsWithDiscounts.Add(newTransaction);
            }

            return transactionsWithDiscounts;
        }
    }
}
