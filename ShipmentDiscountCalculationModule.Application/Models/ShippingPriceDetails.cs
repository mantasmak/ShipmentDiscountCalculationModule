namespace ShipmentDiscountCalculationModule.Application.Models
{
    public class ShippingPriceDetails
    {
        public string Provider { get; set; }
        public string PackageSize { get; set; }
        public decimal Price { get; set; }
    }
}