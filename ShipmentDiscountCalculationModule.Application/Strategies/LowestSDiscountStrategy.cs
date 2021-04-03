using System.Collections.Generic;
using System.Linq;
using ShipmentDiscountCalculationModule.Application.Models;

namespace ShipmentDiscountCalculationModule.Application.Strategies
{
    class LowestSDiscountStrategy : BaseLimitDiscountStrategy
    {
        protected override bool AddDiscountToTransaction(Transaction newTransaction, IEnumerable<Transaction> transactionHistory, decimal remainingDiscount, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            if (newTransaction.Size != "S")
                return false;

            var lowestShippingPrice = shippingPriceDetails.Where(d => d.PackageSize == "S")
                                                          .Min(d => d.Price);

            var shippingPriceBeforeDiscount = shippingPriceDetails.Where(d => d.Provider == newTransaction.Provider)
                                                                  .Where(d => d.PackageSize == "S")
                                                                  .FirstOrDefault()
                                                                  .Price;

            var discount = shippingPriceBeforeDiscount - lowestShippingPrice;

            if (discount == 0)
                return false;

            if (remainingDiscount >= discount)
            {
                newTransaction.ShippingPrice = lowestShippingPrice;
                newTransaction.Discount = discount;

                return true;
            }

            newTransaction.ShippingPrice = shippingPriceBeforeDiscount - remainingDiscount;
            newTransaction.Discount = remainingDiscount;

            return true;
        }
    }
}
