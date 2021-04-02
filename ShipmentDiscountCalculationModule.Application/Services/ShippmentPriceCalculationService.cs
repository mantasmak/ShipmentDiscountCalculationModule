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

        public ShippmentPriceCalculationService(IParser<Transaction> transactionHistoryParser, IValidator transactionValidator, IParser<ShippingPriceDetails> shippingPriceDetailsParser, IValidator shippingPriceDetailsValidator, IDiscountCalculationContext discountCalculationContext)
        {
            _transactionHistoryParser = transactionHistoryParser;
            _transactionValidator = transactionValidator;
            _shippingPriceDetailsParser = shippingPriceDetailsParser;
            _shippingPriceDetailsValidator = shippingPriceDetailsValidator;
            _discountCalculationContext = discountCalculationContext;

        }

        public string GetDiscount(string transactionHistory, string shippingPriceDetails)
        {
            var parsedShippingPriceDetails = _shippingPriceDetailsParser.Parse(shippingPriceDetails, _shippingPriceDetailsValidator);

            var parsedTransactions = _transactionHistoryParser.Parse(transactionHistory, _transactionValidator);

            var transactionsWithPrices = AddPricesBeforeDiscount(parsedTransactions, parsedShippingPriceDetails);

            var transactionsWithDiscount = _discountCalculationContext.ApplyDiscount(transactionsWithPrices, parsedShippingPriceDetails);

            return ConvertTransactionsToString(transactionsWithDiscount, transactionHistory);
        }

        private IEnumerable<Transaction> AddPricesBeforeDiscount(IEnumerable<Transaction> transactions, IEnumerable<ShippingPriceDetails> shippingPriceDetails)
        {
            var transactionsWithPrices = new List<Transaction>();

            foreach(var transaction in transactions)
            {
                var shippingPrice = shippingPriceDetails.Where(d => d.Provider == transaction.Provider)
                                                        .Where(d => d.PackageSize == transaction.Size)
                                                        .Select(d => d.Price)
                                                        .FirstOrDefault();

                transaction.ShippingPrice = shippingPrice;
                transaction.Discount = 0;
                transactionsWithPrices.Add(transaction);
            }

            return transactionsWithPrices;
        }

        private string ConvertTransactionsToString(IEnumerable<Transaction> transactions, string transactionHistory)
        {
            var splitTransactions = transactionHistory.Split("\r\n");
            var stringBuilder = new StringBuilder();

            for (int i = 0 ; i < transactions.Count() ; i++)
            {
                if (transactions.ElementAt(i).WrongTransactionFormat)
                {
                    stringBuilder.AppendLine($"{splitTransactions[i]} Ignored");
                }
                else
                {
                    stringBuilder.AppendLine(transactions.ElementAt(i).ToString());
                }
            }

            return stringBuilder.ToString();
        }
    }
}
