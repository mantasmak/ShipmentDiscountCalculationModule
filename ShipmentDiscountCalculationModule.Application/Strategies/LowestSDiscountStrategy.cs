using System.Collections.Generic;
using System.Linq;
using ShipmentDiscountCalculationModule.Application.Models;

namespace ShipmentDiscountCalculationModule.Application.Strategies
{
    class LowestSDiscountStrategy : BaseLimitDiscountStrategy
    {
        protected override Transaction AddDiscountToTransaction(IEnumerable<Transaction> transactionHistory, Transaction newTransaction, decimal remainingDiscount, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            var lowestShippingPrice = shippingPriceDetails.Where(d => d.PackageSize == newTransaction.Size)
                                                          .Min(d => d.Price);

            var shippingPriceBeforeDiscount = shippingPriceDetails.Where(d => d.Provider == newTransaction.Provider)
                                                                  .Where(d => d.PackageSize == newTransaction.Size)
                                                                  .FirstOrDefault()
                                                                  .Price;

            if (remainingDiscount >= shippingPriceBeforeDiscount - lowestShippingPrice)
            {
                newTransaction.ShippingPrice = lowestShippingPrice;
                newTransaction.Discount = shippingPriceBeforeDiscount - lowestShippingPrice;

                return newTransaction;
            }

            newTransaction.ShippingPrice = shippingPriceBeforeDiscount - remainingDiscount;
            newTransaction.Discount = remainingDiscount;

            return newTransaction;
        }
    }
}
