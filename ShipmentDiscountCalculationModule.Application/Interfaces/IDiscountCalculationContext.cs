using ShipmentDiscountCalculationModule.Application.Models;
using System.Collections.Generic;

namespace ShipmentDiscountCalculationModule.Application.Interfaces
{
    public interface IDiscountCalculationContext
    {
        IEnumerable<Transaction> ApplyDiscount(IEnumerable<Transaction> transactionHistory, IEnumerable<ShippingPriceDetails> shippingPriceDetails);
    }
}
