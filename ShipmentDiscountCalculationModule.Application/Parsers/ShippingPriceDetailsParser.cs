using ShipmentDiscountCalculationModule.Application.Interfaces;
using ShipmentDiscountCalculationModule.Application.Models;
using System;
using System.Linq;

namespace ShipmentDiscountCalculationModule.Application.Parsers
{
    public class ShippingPriceDetailsParser : BaseParser<ShippingPriceDetails>
    {
        public ShippingPriceDetailsParser(IValidator validator) : base(validator) { }

        protected override ShippingPriceDetails GetParsedObject(string textLine)
        {
            if(!_validator.IsValid(textLine))
            {
                return new ShippingPriceDetails();
            }

            var properties = textLine.Split(' ');

            return new ShippingPriceDetails
            {
                Provider = properties[0],
                PackageSize = properties[1],
                Price = Convert.ToDecimal(properties[2])
            };
        }
    }
}
