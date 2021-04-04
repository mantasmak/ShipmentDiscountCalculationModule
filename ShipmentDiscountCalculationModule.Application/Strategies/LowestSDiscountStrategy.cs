using System.Collections.Generic;
using System.Linq;
using ShipmentDiscountCalculationModule.Application.Models;

namespace ShipmentDiscountCalculationModule.Application.Strategies
{
    public class LowestSDiscountStrategy : BaseLimitDiscountStrategy
    {
        protected override bool AddDiscountToTransaction(Transaction transaction, IEnumerable<Transaction> transactionHistory, decimal remainingDiscount, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            if (transaction.Size != "S")
                return false;

            var lowestSShippingPrice = GetLowestSShippingPrice(shippingPriceDetails);

            var shippingPriceBeforeDiscount = GetShippingPriceBeforeDiscount(transaction, shippingPriceDetails);

            var discount = shippingPriceBeforeDiscount - lowestSShippingPrice;

            if (discount == 0)
                return false;
            
            if (remainingDiscount >= discount)
            {
                transaction.ShippingPrice = lowestSShippingPrice;
                transaction.Discount = discount;

                return true;
            }

            transaction.ShippingPrice = shippingPriceBeforeDiscount - remainingDiscount;
            transaction.Discount = remainingDiscount;

            return true;
        }

        private decimal GetLowestSShippingPrice(IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            return shippingPriceDetails.Where(d => d.PackageSize == "S")
                                       .Min(d => d.Price);
        }

        private decimal GetShippingPriceBeforeDiscount(Transaction transaction, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            return shippingPriceDetails.Where(d => d.Provider == transaction.Provider)
                                       .Where(d => d.PackageSize == "S")
                                       .FirstOrDefault()
                                       .Price;
        }
    }
}
