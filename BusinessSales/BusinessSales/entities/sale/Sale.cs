namespace BusinessSales
{
    public class Sale
    {
        public int Id { set; get; }
        public DateOnly Date { set; get; }
        public string NameOfProducts { set; get; }
        public double PriceOfProduct { set; get; }
        public int CountOfProducts { set; get; }
        public string Comment { set; get; }
        public double CostOfSale { set; get; }
        public double PriceOfSale { set; get; }

        public int AccountId { set; get; }
        public Account Account { set; get; }

        public Sale(DateOnly date, string nameOfProducts,
            double priceOfProduct, int countOfProducts, 
            string comment, double costOfSale, int accountId)
        {
            Date = date;
            NameOfProducts = nameOfProducts;
            PriceOfProduct = priceOfProduct;
            CountOfProducts = countOfProducts;
            Comment = comment;
            CostOfSale = costOfSale;
            AccountId = accountId;

            PriceOfSale = priceOfProduct * countOfProducts;
        }
    }
}
