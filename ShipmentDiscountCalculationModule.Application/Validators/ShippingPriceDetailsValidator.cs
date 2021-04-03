using ShipmentDiscountCalculationModule.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ShipmentDiscountCalculationModule.Application.Validators
{
    public class ShippingPriceDetailsValidator : IValidator
    {
        public bool IsValid(IEnumerable<string> textLine)
        {
            if (textLine.Count() != 3)
                return false;

            if (!IsProviderValid(textLine.ElementAt(0)))
                return false;

            if (!IsSizeValid(textLine.ElementAt(1)))
                return false;

            if(!IsPriceValid(textLine.ElementAt(2)))
                return false;

            return true;
        }

        private bool IsProviderValid(string provider) => provider == "LP" || provider == "MR";

        private bool IsSizeValid(string size) => size == "S" || size == "M" || size == "L";

        private bool IsPriceValid(string price) => decimal.TryParse(price, out _);
    }
}
