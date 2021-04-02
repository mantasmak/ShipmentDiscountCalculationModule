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
        public bool WrongTransactionFormat { get; set; } = false;

        public override string ToString() => Discount == 0 ? $"{Date.ToShortDateString()} {Size} {Provider} {ShippingPrice:N2} -" 
                                                           : $"{Date.ToShortDateString()} {Size} {Provider} {ShippingPrice:N2} {Discount:N2}";
    }
}
