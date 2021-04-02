using ShipmentDiscountCalculationModule.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipmentDiscountCalculationModule.Application.Parsers
{
    public class TransactionHistoryParser : BaseParser<Transaction>
    {
        protected override Transaction GetNewConcreteObject(IEnumerable<string> properties)
        {
            return new Transaction
            {
                Date = DateTime.Parse(properties.ElementAt(0)),
                Size = properties.ElementAt(1),
                Provider = properties.ElementAt(2)
            };
        }

        protected override Transaction GetInvalidObject()
        {
            return new Transaction
            {
                Date = new DateTime(),
                Size = String.Empty,
                Provider = String.Empty,
                ShippingPrice = 0,
                Discount = 0,
                WrongTransactionFormat = true
            };
        }
    }
}
