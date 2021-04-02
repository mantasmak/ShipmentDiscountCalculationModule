using ShipmentDiscountCalculationModule.Application.Interfaces;
using System;

namespace ShipmentDiscountCalculationModule.Application.Validators
{
    public class TransactionValidator : IValidator
    {
        public bool IsValid(string text)
        {
            var transactionElements = text.Split(' ');

            if (transactionElements.Length != 3)
                return false;

            if (!IsDateValid(transactionElements[0]))
                return false;

            if (!IsSizeValid(transactionElements[1]))
                return false;

            if (!IsProviderValid(transactionElements[2]))
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
