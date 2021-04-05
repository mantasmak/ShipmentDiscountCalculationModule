using ShipmentDiscountCalculationModule.Application.Interfaces;
using ShipmentDiscountCalculationModule.Application.Models;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System;

namespace ShipmentDiscountCalculationModule.Application.Services
{
    public class ShippmentPriceCalculationService : IShippmentPriceCalculationService
    {
        private readonly IParser<Transaction> _transactionHistoryParser;
        private readonly IParser<ShippingPriceDetails> _shippingPriceDetailsParser;
        private readonly IDiscountStrategyContext _discountStrategyContext;

        public ShippmentPriceCalculationService(
            IParser<Transaction> transactionHistoryParser, IParser<ShippingPriceDetails> shippingPriceDetailsParser, IDiscountStrategyContext discountStrategyContext)
        {
            _transactionHistoryParser = transactionHistoryParser;
            _shippingPriceDetailsParser = shippingPriceDetailsParser;
            _discountStrategyContext = discountStrategyContext;

        }

        public string AddDiscount(string transactionHistory, string shippingPriceDetails)
        {
            if (transactionHistory == null || shippingPriceDetails == null)
                throw new ArgumentNullException();

            var parsedShippingPriceDetails = _shippingPriceDetailsParser.Parse(shippingPriceDetails);
            var parsedTransactions = _transactionHistoryParser.Parse(transactionHistory);

            AddPricesBeforeDiscount(parsedTransactions, parsedShippingPriceDetails);

            _discountStrategyContext.ApplyDiscount(parsedTransactions, parsedShippingPriceDetails);

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
