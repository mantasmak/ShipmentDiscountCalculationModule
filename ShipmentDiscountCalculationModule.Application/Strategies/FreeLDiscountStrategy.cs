using System.Collections.Generic;
using System.Linq;
using ShipmentDiscountCalculationModule.Application.Models;

namespace ShipmentDiscountCalculationModule.Application.Strategies
{
    class FreeLDiscountStrategy : BaseLimitDiscountStrategy
    {
        protected override Transaction AddDiscountToTransaction(IEnumerable<Transaction> transactionHistory, Transaction newTransaction, decimal remainingDiscount, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            var currentMonth = newTransaction.Date.Month;

            var numOfEligibleMonths = transactionHistory.Where(t => t.Date.Month == currentMonth)
                                                        .Where(t => t.Size == newTransaction.Size)
                                                        .Where(t => t.Provider == newTransaction.Provider)
                                                        .Count();

            var shippingPrice = shippingPriceDetails.Where(d => d.Provider == newTransaction.Provider)
                                                       .Where(d => d.PackageSize == newTransaction.Size)
                                                       .Select(d => d.Price)
                                                       .FirstOrDefault();

            if (numOfEligibleMonths == 2)
            {
                if(remainingDiscount >= shippingPrice)
                {
                    newTransaction.ShippingPrice = 0;
                    newTransaction.Discount = shippingPrice;

                    return newTransaction;
                }

                newTransaction.ShippingPrice = shippingPrice - remainingDiscount;
                newTransaction.Discount = remainingDiscount;

                return newTransaction;
            }

            newTransaction.ShippingPrice = shippingPrice;
            newTransaction.Discount = 0;

            return newTransaction;
        }
    }
}
