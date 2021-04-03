using System.Collections.Generic;
using ShipmentDiscountCalculationModule.Application.Models;

namespace ShipmentDiscountCalculationModule.Application.Interfaces
{
    interface IDiscountStrategy
    {
        bool TryApplyDiscount(Transaction newTransaction, IEnumerable<Transaction> transactionHistory, IEnumerable<ShippingPriceDetails> shippingPriceDetails);
    }
}
