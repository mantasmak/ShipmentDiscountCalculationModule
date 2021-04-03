using System.Collections.Generic;
using System.Linq;
using ShipmentDiscountCalculationModule.Application.Models;

namespace ShipmentDiscountCalculationModule.Application.Strategies
{
    class FreeLDiscountStrategy : BaseLimitDiscountStrategy
    {
        protected override bool AddDiscountToTransaction(Transaction newTransaction, IEnumerable<Transaction> transactionHistory, decimal remainingDiscount, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            if (newTransaction.Size != "L" || newTransaction.Provider != "LP")
                return false;

            var numOfEligibleMonths = transactionHistory.Where(t => t.Date.Month == newTransaction.Date.Month)
                                                        .Where(t => t.Date < newTransaction.Date)
                                                        .Where(t => t.Size == "L")
                                                        .Where(t => t.Provider == "LP")
                                                        .Count();
            if (numOfEligibleMonths != 2)
                return false;

            var shippingPrice = shippingPriceDetails.Where(d => d.Provider == newTransaction.Provider)
                                                       .Where(d => d.PackageSize == newTransaction.Size)
                                                       .Select(d => d.Price)
                                                       .FirstOrDefault();

            if(remainingDiscount >= shippingPrice)
            {
                newTransaction.ShippingPrice = 0;
                newTransaction.Discount = shippingPrice;

                return true;
            }

            newTransaction.ShippingPrice = shippingPrice - remainingDiscount;
            newTransaction.Discount = remainingDiscount;

            return true;
        }
    }
}
