namespace BusinessSales
{
    public record PurchaseResponseJson(string id, string date, string nameOfProducts,
        string priceOfProduct, string countOfProducts,
        string comment, string priceOfPurchase);
}
