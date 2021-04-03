using System;

namespace ShipmentDiscountCalculationModule.Application.Models
{
    public class Transaction
    {
        public DateTime Date { get; set; }
        public string Size { get; set; }
        public string Provider { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal Discount { get; set; }
        public string RawText { get; set; }
        public bool WrongTransactionFormat { get; set; } = false;

        public override string ToString()
        {
            if (WrongTransactionFormat)
                return $"{RawText} Ignored";

            if (Discount == 0)
                return $"{Date.ToShortDateString()} {Size} {Provider} {ShippingPrice:N2} -";
            else
                return $"{Date.ToShortDateString()} {Size} {Provider} {ShippingPrice:N2} {Discount:N2}";
        }
    }
}
