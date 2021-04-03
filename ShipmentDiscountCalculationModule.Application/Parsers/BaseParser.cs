using ShipmentDiscountCalculationModule.Application.Interfaces;
using System.Collections.Generic;

namespace ShipmentDiscountCalculationModule.Application.Parsers
{
    public abstract class BaseParser<T> : IParser<T>
    {
        public IEnumerable<T> Parse(string text, IValidator validator = null)
        {
            var parsedText = new List<T>();

            var textLines = text.Split("\r\n");

            foreach (var textLine in textLines)
            {
                var textLineElements = textLine.Split(' ');
                parsedText.Add(GetNewConcreteObject(textLineElements, validator));
            }

            return parsedText;
        }

        protected abstract T GetNewConcreteObject(IEnumerable<string> properties, IValidator validator = null);
    }
}
