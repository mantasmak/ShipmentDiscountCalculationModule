using System.Collections.Generic;

namespace ShipmentDiscountCalculationModule.Application.Interfaces
{
    public interface IValidator
    {
        public bool IsValid(IEnumerable<string> textLine);
    }
}
