namespace ShipmentDiscountCalculationModule.Application.Interfaces
{
    public interface IShippmentPriceCalculationService
    {
        string GetDiscount(string transactionHistory, string shippingPriceDetails);
    }
}
