using ShipmentDiscountCalculationModule.Application.Interfaces;
using ShipmentDiscountCalculationModule.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipmentDiscountCalculationModule.Application.Parsers
{
    public class TransactionHistoryParser : BaseParser<Transaction>
    {
        protected override Transaction GetNewConcreteObject(IEnumerable<string> properties, IValidator validator = null)
        {
            if (validator != null)
            {
                if (!validator.IsValid(properties))
                {
                    return new Transaction
                    {
                        WrongTransactionFormat = true,
                        RawText = String.Join(" ", properties)
                    };
                }
            }

            return new Transaction
            {
                Date = DateTime.Parse(properties.ElementAt(0)),
                Size = properties.ElementAt(1),
                Provider = properties.ElementAt(2),
                RawText = String.Join(" ", properties)
            };
        }
    }
}
