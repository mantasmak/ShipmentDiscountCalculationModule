using ShipmentDiscountCalculationModule.Application.Interfaces;
using System;
using System.Collections.Generic;

namespace ShipmentDiscountCalculationModule.Application.Parsers
{
    public abstract class BaseParser<T> : IParser<T>
    {
        protected readonly IValidator _validator;

        public BaseParser(IValidator validator)
        {
            _validator = validator;
        }

        public IEnumerable<T> Parse(string text)
        {
            if (text == null)
                throw new ArgumentNullException();
            
            var parsedText = new List<T>();
            
            var textLines = text.Split(Environment.NewLine);

            foreach (var textLine in textLines)
            {
                parsedText.Add(GetParsedObject(textLine));
            }

            return parsedText;
        }

        protected abstract T GetParsedObject(string textLine);
    }
}
