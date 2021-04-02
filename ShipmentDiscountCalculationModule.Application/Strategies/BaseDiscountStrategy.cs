using ShipmentDiscountCalculationModule.Application.Interfaces;
using System.Linq;
using System.Collections.Generic;
using ShipmentDiscountCalculationModule.Application.Models;

namespace ShipmentDiscountCalculationModule.Application.Strategies
{
    abstract class BaseLimitDiscountStrategy : IDiscountStrategy
    {
        private const decimal _accumulatedDiscountLimit = 10;

        public Transaction CalculateDiscount(IEnumerable<Transaction> transactionHistory, Transaction newTransaction, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            var remainingDiscount = GetRemainingDiscount(transactionHistory, newTransaction);

            return AddDiscountToTransaction(transactionHistory, newTransaction, remainingDiscount, shippingPriceDetails);
        }

        private decimal GetRemainingDiscount(IEnumerable<Transaction> transactionHistory, Transaction newTransaction)
        {
            if (!transactionHistory.Any())
                return _accumulatedDiscountLimit;

            var accumulatedDiscount = transactionHistory.Where(t => t.Date.Month == newTransaction.Date.Month).Sum(t => t.Discount);

            return _accumulatedDiscountLimit - accumulatedDiscount;
        }

        abstract protected Transaction AddDiscountToTransaction(IEnumerable<Transaction> transactionHistory, Transaction newTransaction, decimal remainingDiscount, IEnumerable<ShippingPriceDetails> shippingPriceDetails);
    }
}
