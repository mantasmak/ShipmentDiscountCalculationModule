using ShipmentDiscountCalculationModule.Application.Interfaces;
using System.Linq;
using System.Collections.Generic;
using ShipmentDiscountCalculationModule.Application.Models;
using System;

namespace ShipmentDiscountCalculationModule.Application.Strategies
{
    abstract class BaseLimitDiscountStrategy : IDiscountStrategy
    {
        private const decimal _accumulatedDiscountLimit = 10;

        public bool TryApplyDiscount(Transaction transaction, IEnumerable<Transaction> transactionHistory, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            var remainingDiscount = GetRemainingDiscount(transactionHistory, transaction.Date);

            if (remainingDiscount == 0)
                return false;
            
            return AddDiscountToTransaction(transaction, transactionHistory, remainingDiscount, shippingPriceDetails);
        }

        private decimal GetRemainingDiscount(IEnumerable<Transaction> transactionHistory, DateTime transactionDate)
        {
            var accumulatedDiscount = transactionHistory.Where(t => t.Date.Month == transactionDate.Month)
                                                        .Where(t => t.Date < transactionDate)
                                                        .Sum(t => t.Discount);

            return _accumulatedDiscountLimit - accumulatedDiscount;
        }

        abstract protected bool AddDiscountToTransaction(Transaction newTransaction, IEnumerable<Transaction> transactionHistory, decimal remainingDiscount, IEnumerable<ShippingPriceDetails> shippingPriceDetails);
    }
}
