using ShipmentDiscountCalculationModule.Application.Interfaces;
using System.Linq;
using System.Collections.Generic;
using ShipmentDiscountCalculationModule.Application.Models;
using System;

namespace ShipmentDiscountCalculationModule.Application.Strategies
{
    public abstract class BaseLimitDiscountStrategy : IDiscountStrategy
    {
        private const decimal _accumulatedDiscountLimit = 10;

        public bool TryApplyDiscount(Transaction transaction, IEnumerable<Transaction> transactionHistory, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            if (transaction == null || transactionHistory == null || shippingPriceDetails == null)
                throw new ArgumentNullException();

            if (transaction.IsEmpty() || !transactionHistory.Any() || !shippingPriceDetails.Any())
                throw new ArgumentException();

            var remainingDiscount = GetRemainingDiscount(transactionHistory, transaction.Date);

            if (remainingDiscount <= 0)
                return false;
            
            return AddDiscountToTransaction(transaction, transactionHistory, remainingDiscount, shippingPriceDetails);
        }

        private decimal GetRemainingDiscount(IEnumerable<Transaction> transactionHistory, DateTime transactionDate)
        {
            var accumulatedDiscount = GetAccumulatedDiscount(transactionHistory, transactionDate);

            return _accumulatedDiscountLimit - accumulatedDiscount;
        }

        private decimal GetAccumulatedDiscount(IEnumerable<Transaction> transactionHistory, DateTime transactionDate)
        {
            return transactionHistory.Where(t => t.Date.Month == transactionDate.Month)
                                     .Where(t => t.Date <= transactionDate)
                                     .Sum(t => t.Discount);
        }

        abstract protected bool AddDiscountToTransaction(Transaction transaction, IEnumerable<Transaction> transactionHistory, decimal remainingDiscount, IEnumerable<ShippingPriceDetails> shippingPriceDetails);
    }
}
