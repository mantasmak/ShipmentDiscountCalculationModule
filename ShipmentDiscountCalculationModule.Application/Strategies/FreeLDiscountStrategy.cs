using System.Collections.Generic;
using System.Linq;
using ShipmentDiscountCalculationModule.Application.Models;

namespace ShipmentDiscountCalculationModule.Application.Strategies
{
    class FreeLDiscountStrategy : BaseLimitDiscountStrategy
    {
        protected override bool AddDiscountToTransaction(Transaction transaction, IEnumerable<Transaction> transactionHistory, decimal remainingDiscount, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            if (transaction.Size != "L" || transaction.Provider != "LP")
                return false;

            var numOfEligibleMonths = transactionHistory.Where(t => t.Date.Month == transaction.Date.Month)
                                                        .Where(t => t.Date < transaction.Date)
                                                        .Where(t => t.Size == "L")
                                                        .Where(t => t.Provider == "LP")
                                                        .Count();
            if (numOfEligibleMonths != 2)
                return false;

            var shippingPrice = shippingPriceDetails.Where(d => d.Provider == transaction.Provider)
                                                       .Where(d => d.PackageSize == transaction.Size)
                                                       .Select(d => d.Price)
                                                       .FirstOrDefault();

            if(remainingDiscount >= shippingPrice)
            {
                transaction.ShippingPrice = 0;
                transaction.Discount = shippingPrice;

                return true;
            }

            transaction.ShippingPrice = shippingPrice - remainingDiscount;
            transaction.Discount = remainingDiscount;

            return true;
        }
    }
}
