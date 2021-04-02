using System.Collections.Generic;
using ShipmentDiscountCalculationModule.Application.Models;

namespace ShipmentDiscountCalculationModule.Application.Interfaces
{
    interface IDiscountStrategy
    {
        Transaction CalculateDiscount(IEnumerable<Transaction> transactionHistory, Transaction newTransaction, IEnumerable<ShippingPriceDetails> shippingPriceDetails);
    }
}
