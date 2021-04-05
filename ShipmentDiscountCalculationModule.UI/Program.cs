using ShipmentDiscountCalculationModule.Application.Parsers;
using ShipmentDiscountCalculationModule.Application.Validators;
using ShipmentDiscountCalculationModule.Application.Services;
using ShipmentDiscountCalculationModule.Application.Strategies;
using System;
using System.Linq;
using System.IO;

namespace ShipmentDiscountCalculationModule.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFileName = "input.txt";
            var shippingPriceDetailsFileName = "shippingPriceDetails.txt";

            if (args.Length == 2)
            {
                inputFileName = args[0];
                shippingPriceDetailsFileName = args[1];
            }

            var inputPath = Path.Combine(Environment.CurrentDirectory, @"Data\", inputFileName);
            var shippingPriceDetailsPath = Path.Combine(Environment.CurrentDirectory, @"Data\", shippingPriceDetailsFileName);

            var transactionHistory = ReadFile(inputPath);
            var shippingPriceDetails = ReadFile(shippingPriceDetailsPath);

            if (String.IsNullOrEmpty(transactionHistory) || String.IsNullOrEmpty(shippingPriceDetails))
                return;

            var transactionHistoryParser = new TransactionHistoryParser(new TransactionValidator());
            var shippingPriceDetailsParser = new ShippingPriceDetailsParser(new ShippingPriceDetailsValidator());

            var shippmentCalculationService = new ShippmentPriceCalculationService(transactionHistoryParser, shippingPriceDetailsParser, new DiscountStrategyContext());

            Console.WriteLine(shippmentCalculationService.AddDiscount(transactionHistory, shippingPriceDetails));
        }

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
