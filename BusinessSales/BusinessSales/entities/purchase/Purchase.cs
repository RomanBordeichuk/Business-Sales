namespace BusinessSales
{
    public class Purchase
    {
        public int Id { set; get; }
        public DateOnly Date { set; get; }
        public string NameOfProducts { set; get; }
        public double PriceOfProduct { set; get; }
        public int CountOfProducts { set; get; }
        public string Comment { set; get; }
        public double PriceOfPurchase { set; get; }

        public int AccountId { set; get; }
        public Account Account { set; get; }

        public Purchase(DateOnly date, string nameOfProducts, 
            double priceOfProduct, int countOfProducts, 
            string comment, int accountId)
        {
            Date = date;
            NameOfProducts = nameOfProducts;
            PriceOfProduct = priceOfProduct;
            CountOfProducts = countOfProducts;
            Comment = comment;
            AccountId = accountId;

            PriceOfPurchase = priceOfProduct * countOfProducts;
        }
    }
}
