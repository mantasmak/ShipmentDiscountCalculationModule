using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ShipmentDiscountCalculationModule.Application.Tests.Strategies
{
    class DiscountStrategyTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            var transactionsPath = Path.Combine(Environment.CurrentDirectory, @"Data\", "transactions.txt");
            var shippingPriceDetailsPath = Path.Combine(Environment.CurrentDirectory, @"Data\", "shippingPriceDetails.txt");

            var transactions = ReadFile(transactionsPath);

            yield return new object[] { 1, 2, 3 };
            yield return new object[] { -4, -6, -10 };
            yield return new object[] { -2, 2, 0 };
            yield return new object[] { int.MinValue, -1, int.MaxValue };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        static private string ReadFile(string path)
        {
            var fileContent = String.Empty;

            try
            {
                using var streamReader = new StreamReader(path);
                fileContent = streamReader.ReadToEnd();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return fileContent;
        }
    }
}
