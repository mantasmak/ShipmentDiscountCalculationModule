using ShipmentDiscountCalculationModule.Application.Interfaces;
using System;
using System.Linq;

namespace ShipmentDiscountCalculationModule.Application.Validators
{
    public class TransactionValidator : IValidator
    {
        public bool IsValid(string text)
        {
            if (text == null)
                throw new ArgumentNullException();

            var splitText = text.Split(" ");

            if (splitText.Count() != 3)
                return false;

            if (!IsDateValid(splitText[0]))
                return false;

            if (!IsSizeValid(splitText[1]))
                return false;

            if (!IsProviderValid(splitText[2]))
                return false;

            return true;
        }

        private bool IsDateValid(string date)
        {
            string format = "yyyy-MM-dd";

            if (!DateTime.TryParseExact(date, format, null, System.Globalization.DateTimeStyles.None, out _))
                return false;

            return true;
        }

        private bool IsProviderValid(string provider) => provider == "LP" || provider == "MR";

        private bool IsSizeValid(string size) => size == "S" || size == "M" || size == "L";
    }
}
