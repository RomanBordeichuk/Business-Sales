namespace BusinessSales
{
    public class ProductsBatch
    {
        public int Id { set; get; }
        public string NameOfProducts { set; get; }
        public int CountOfProducts { set; get; }
        public double PurchasePrice { set; get; }

        public int AccountId { set; get; }
        public Account Account { set; get; }

        public ProductsBatch(string nameOfProducts, 
            int countOfProducts, double purchasePrice, int accountId)
        {
            NameOfProducts = nameOfProducts;
            CountOfProducts = countOfProducts;
            PurchasePrice = purchasePrice;
            AccountId = accountId;
        }
    }
}
