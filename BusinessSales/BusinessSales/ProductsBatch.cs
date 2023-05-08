namespace BusinessSales
{
    record ProductsBatchJson(string nameOfProducts, 
        string countOfProducts, string purchasePrice);

    public class ProductsBatch
    {
        private int id;
        private string nameOfProducts;
        private int countOfProducts;
        private double purchasePrice;

        public int Id
        {
            set { }
            get { return id; }
        }
        public string NameOfProducts
        {
            set { }
            get { return nameOfProducts; }
        }
        public int CountOfProducts
        {
            set { countOfProducts = value; }
            get { return countOfProducts; }
        }
        public double PurchasePrice
        {
            set { purchasePrice = value; }
            get { return purchasePrice; }
        }

        public ProductsBatch(string nameOfProducts, 
            int countOfProducts, double purchasePrice)
        {
            this.nameOfProducts = nameOfProducts;
            this.countOfProducts = countOfProducts;
            this.purchasePrice = purchasePrice;
        }
    }
}
