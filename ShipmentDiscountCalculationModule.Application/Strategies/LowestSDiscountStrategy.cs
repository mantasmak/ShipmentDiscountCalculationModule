using System.Collections.Generic;
using System.Linq;
using ShipmentDiscountCalculationModule.Application.Models;

namespace ShipmentDiscountCalculationModule.Application.Strategies
{
    class LowestSDiscountStrategy : BaseLimitDiscountStrategy
    {
        protected override bool AddDiscountToTransaction(Transaction transaction, IEnumerable<Transaction> transactionHistory, decimal remainingDiscount, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            if (transaction.Size != "S")
                return false;

            var lowestShippingPrice = shippingPriceDetails.Where(d => d.PackageSize == "S")
                                                          .Min(d => d.Price);

            var shippingPriceBeforeDiscount = shippingPriceDetails.Where(d => d.Provider == transaction.Provider)
                                                                  .Where(d => d.PackageSize == "S")
                                                                  .FirstOrDefault()
                                                                  .Price;

            var discount = shippingPriceBeforeDiscount - lowestShippingPrice;

            if (discount == 0)
                return false;
            
            if (remainingDiscount >= discount)
            {
                transaction.ShippingPrice = lowestShippingPrice;
                transaction.Discount = discount;

                return true;
            }

            transaction.ShippingPrice = shippingPriceBeforeDiscount - remainingDiscount;
            transaction.Discount = remainingDiscount;

            return true;
        }
    }
}
