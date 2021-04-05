using ShipmentDiscountCalculationModule.Application.Interfaces;
using System;
using System.Linq;

namespace ShipmentDiscountCalculationModule.Application.Validators
{
    public class ShippingPriceDetailsValidator : IValidator
    {
        public bool IsValid(string text)
        {
            if (text == null)
                throw new ArgumentNullException();

            var splitText = text.Split(" ");

            if (splitText.Count() != 3)
                return false;

            if (!IsProviderValid(splitText[0]))
                return false;

            if (!IsSizeValid(splitText[1]))
                return false;

            if(!IsPriceValid(splitText[2]))
                return false;

            return true;
        }

        private bool IsProviderValid(string provider) => provider == "LP" || provider == "MR";

        private bool IsSizeValid(string size) => size == "S" || size == "M" || size == "L";

        private bool IsPriceValid(string price)
        {
            if (!decimal.TryParse(price, out decimal parsedNumber))
                return false;

            if (parsedNumber <= 0)
                return false;

            return true;
        }
    }
}
