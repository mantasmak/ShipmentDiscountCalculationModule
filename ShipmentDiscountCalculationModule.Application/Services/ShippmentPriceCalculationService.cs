using ShipmentDiscountCalculationModule.Application.Interfaces;
using ShipmentDiscountCalculationModule.Application.Models;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ShipmentDiscountCalculationModule.Application.Services
{
    public class ShippmentPriceCalculationService : IShippmentPriceCalculationService
    {
        private readonly IParser<Transaction> _transactionHistoryParser;
        private readonly IValidator _transactionValidator;
        private readonly IParser<ShippingPriceDetails> _shippingPriceDetailsParser;
        private readonly IValidator _shippingPriceDetailsValidator;
        private readonly IDiscountCalculationContext _discountCalculationContext;

        public ShippmentPriceCalculationService(
            IParser<Transaction> transactionHistoryParser, IValidator transactionValidator, 
            IParser<ShippingPriceDetails> shippingPriceDetailsParser, IValidator shippingPriceDetailsValidator, 
            IDiscountCalculationContext discountCalculationContext)
        {
            _transactionHistoryParser = transactionHistoryParser;
            _transactionValidator = transactionValidator;
            _shippingPriceDetailsParser = shippingPriceDetailsParser;
            _shippingPriceDetailsValidator = shippingPriceDetailsValidator;
            _discountCalculationContext = discountCalculationContext;

        }

        public string AddDiscount(string transactionHistory, string shippingPriceDetails)
        {
            var parsedShippingPriceDetails = _shippingPriceDetailsParser.Parse(shippingPriceDetails, _shippingPriceDetailsValidator);
            var parsedTransactions = _transactionHistoryParser.Parse(transactionHistory, _transactionValidator);

            AddPricesBeforeDiscount(parsedTransactions, parsedShippingPriceDetails);

            _discountCalculationContext.ApplyDiscount(parsedTransactions, parsedShippingPriceDetails);

            return ConvertTransactionsToString(parsedTransactions);
        }

        private void AddPricesBeforeDiscount(IEnumerable<Transaction> transactions, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            foreach(var transaction in transactions)
            {
                var shippingPrice = GetShipShippingPrice(transaction, shippingPriceDetails);

                transaction.ShippingPrice = shippingPrice;
                transaction.Discount = 0;
            }
        }

        private decimal GetShipShippingPrice(Transaction transaction, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            return shippingPriceDetails.Where(d => d.Provider == transaction.Provider)
                                       .Where(d => d.PackageSize == transaction.Size)
                                       .Select(d => d.Price)
                                       .FirstOrDefault();
        }

        private string ConvertTransactionsToString(IEnumerable<Transaction> transactions)
        {
            var stringBuilder = new StringBuilder();

            foreach(var transaction in transactions)
            {
                stringBuilder.AppendLine(transaction.ToString());
            }

            return stringBuilder.ToString();
        }
    }
}
