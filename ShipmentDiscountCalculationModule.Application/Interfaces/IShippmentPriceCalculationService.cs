namespace ShipmentDiscountCalculationModule.Application.Interfaces
{
    public interface IShippmentPriceCalculationService
    {
        string AddDiscount(string transactionHistory, string shippingPriceDetails);
    }
}
