using ShipmentDiscountCalculationModule.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipmentDiscountCalculationModule.Application.Parsers
{
    public class ShippingPriceDetailsParser : BaseParser<ShippingPriceDetails>
    {
        protected override ShippingPriceDetails GetNewConcreteObject(IEnumerable<string> properties)
        {
            return new ShippingPriceDetails
            {
                Provider = properties.ElementAt(0),
                PackageSize = properties.ElementAt(1),
                Price = Convert.ToDecimal(properties.ElementAt(2))
            };
        }

        protected override ShippingPriceDetails GetInvalidObject()
        {
            return new ShippingPriceDetails
            {
                Provider = String.Empty,
                PackageSize = String.Empty,
                Price = 0
            };
        }
    }
}
