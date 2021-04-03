using ShipmentDiscountCalculationModule.Application.Interfaces;
using ShipmentDiscountCalculationModule.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipmentDiscountCalculationModule.Application.Parsers
{
    public class ShippingPriceDetailsParser : BaseParser<ShippingPriceDetails>
    {
        protected override ShippingPriceDetails GetNewConcreteObject(IEnumerable<string> properties, IValidator validator = null)
        {
            if(validator != null)
            {
                if(!validator.IsValid(properties))
                {
                    return new ShippingPriceDetails();
                }
            }

            return new ShippingPriceDetails
            {
                Provider = properties.ElementAt(0),
                PackageSize = properties.ElementAt(1),
                Price = Convert.ToDecimal(properties.ElementAt(2))
            };
        }
    }
}
