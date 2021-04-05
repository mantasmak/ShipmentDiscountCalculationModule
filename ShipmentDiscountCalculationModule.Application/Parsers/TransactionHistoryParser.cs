using ShipmentDiscountCalculationModule.Application.Interfaces;
using ShipmentDiscountCalculationModule.Application.Models;
using System;
using System.Linq;

namespace ShipmentDiscountCalculationModule.Application.Parsers
{
    public class TransactionHistoryParser : BaseParser<Transaction>
    {
        public TransactionHistoryParser(IValidator validator) : base(validator) { }

        protected override Transaction GetParsedObject(string textLine)
        {
            if (!_validator.IsValid(textLine))
            {
                return new Transaction
                {
                    WrongTransactionFormat = true,
                    RawText = textLine
                };
            }

            var properties = textLine.Split(' ');

            return new Transaction
            {
                Date = DateTime.Parse(properties[0]),
                Size = properties[1],
                Provider = properties[2],
                RawText = String.Join(" ", properties)
            };
        }
    }
}
