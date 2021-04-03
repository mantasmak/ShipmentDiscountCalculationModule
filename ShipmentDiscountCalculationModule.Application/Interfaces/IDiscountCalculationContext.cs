using ShipmentDiscountCalculationModule.Application.Models;
using System.Collections.Generic;

namespace ShipmentDiscountCalculationModule.Application.Interfaces
{
    public interface IDiscountCalculationContext
    {
        void ApplyDiscount(IEnumerable<Transaction> transactionHistory, IEnumerable<ShippingPriceDetails> shippingPriceDetails);
    }
}
