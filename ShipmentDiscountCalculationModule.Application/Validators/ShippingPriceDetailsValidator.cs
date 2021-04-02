using ShipmentDiscountCalculationModule.Application.Interfaces;

namespace ShipmentDiscountCalculationModule.Application.Validators
{
    public class ShippingPriceDetailsValidator : IValidator
    {
        public bool IsValid(string text)
        {
            var detailElements = text.Split(' ');

            if (detailElements.Length != 3)
                return false;

            if (!IsProviderValid(detailElements[0]))
                return false;

            if (!IsSizeValid(detailElements[1]))
                return false;

            return true;
        }

        private bool IsProviderValid(string provider) => provider == "LP" || provider == "MR";

        private bool IsSizeValid(string size) => size == "S" || size == "M" || size == "L";
    }
}
