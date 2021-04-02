using System.Collections.Generic;

namespace ShipmentDiscountCalculationModule.Application.Interfaces
{
    public interface IParser<T>
    {
        public IEnumerable<T> Parse(string text, IValidator validator = null);
    }
}
