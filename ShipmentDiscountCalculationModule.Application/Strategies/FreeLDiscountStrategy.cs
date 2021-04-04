using System.Collections.Generic;
using System.Linq;
using ShipmentDiscountCalculationModule.Application.Models;

namespace ShipmentDiscountCalculationModule.Application.Strategies
{
    public class FreeLDiscountStrategy : BaseLimitDiscountStrategy
    {
        protected override bool AddDiscountToTransaction(Transaction transaction, IEnumerable<Transaction> transactionHistory, decimal remainingDiscount, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            if (!IsTransactionEligibleForDiscount(transaction, transactionHistory))
                return false;

            var shippingPrice = GetShippingPrice(transaction, shippingPriceDetails);

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

        private bool IsTransactionEligibleForDiscount(Transaction transaction, IEnumerable<Transaction> transactionHistory)
        {
            if (transaction.Size != "L" || transaction.Provider != "LP")
                return false;

            var numOfEligibleMonths = GetNumOfEligibleMonths(transaction, transactionHistory);

            if (numOfEligibleMonths != 2)
                return false;

            return true;
        }

        private decimal GetNumOfEligibleMonths(Transaction transaction, IEnumerable<Transaction> transactionHistory)
        {
            return transactionHistory.Where(t => t.Date.Month == transaction.Date.Month)
                                     .Where(t => t.Date <= transaction.Date)
                                     .Where(t => t.Size == "L")
                                     .Where(t => t.Provider == "LP")
                                     .Count();
        }

        private decimal GetShippingPrice(Transaction transaction, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            return shippingPriceDetails.Where(d => d.Provider == transaction.Provider)
                                       .Where(d => d.PackageSize == transaction.Size)
                                       .Select(d => d.Price)
                                       .FirstOrDefault();
        }
    }
}
