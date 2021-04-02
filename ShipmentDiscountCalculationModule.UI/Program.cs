using ShipmentDiscountCalculationModule.Application.Parsers;
using ShipmentDiscountCalculationModule.Application.Validators;
using ShipmentDiscountCalculationModule.Application.Services;
using ShipmentDiscountCalculationModule.Application.Strategies;
using System;
using System.IO;

namespace ShipmentDiscountCalculationModule.UI
{
    class Program
    {
        static void Main()
        {
            string transactionHistory = String.Empty;
            string shippingPriceDetails = String.Empty;

            try
            {
                var inputPath = Path.Combine(Environment.CurrentDirectory, @"Data\", "input.txt");

                using (var streamReader = new StreamReader(inputPath))
                {
                    transactionHistory = streamReader.ReadToEnd();
                }

                var shippingPriceDetailsPath = Path.Combine(Environment.CurrentDirectory, @"Data\", "shippingPriceDetails.txt");

                using (var streamReader = new StreamReader(shippingPriceDetailsPath))
                {
                    shippingPriceDetails = streamReader.ReadToEnd();
                }
            }
            catch(FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (String.IsNullOrEmpty(transactionHistory) || String.IsNullOrEmpty(shippingPriceDetails))
                return;

            var shippmentCalculationService = new ShippmentPriceCalculationService(new TransactionHistoryParser(), new TransactionValidator(), new ShippingPriceDetailsParser(), new ShippingPriceDetailsValidator(), new DiscountCalculationContext());

            Console.WriteLine(shippmentCalculationService.GetDiscount(transactionHistory, shippingPriceDetails));
        }
    }
}
