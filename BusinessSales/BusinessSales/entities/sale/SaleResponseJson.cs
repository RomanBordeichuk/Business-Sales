namespace BusinessSales
{
    public record SaleResponseJson(string id, string date, string nameOfProducts,
        string priceOfProduct, string countOfProducts,
        string comment, string priceOfSale, string costOfSale);
}
