namespace BusinessSales
{
    record PurchaseJson(string date, string nameOfProducts, 
        string priceOfProduct, string countOfProducts, string comment);
    record PurchaseResponseJson(string date, string nameOfProducts, 
        string priceOfProduct, string countOfProducts, 
        string comment, string priceOfPurchase);

    public class Purchase
    {
        private int id;
        private DateOnly date;
        private string nameOfProducts;
        private double priceOfProduct;
        private int countOfProducts;
        private string comment;

        private double priceOfPurchase;

        public int Id
        {
            set { id = value; }
            get { return id; }
        }
        public DateOnly Date
        {
            set { }
            get { return date; }
        }
        public string NameOfProducts
        {
            set { }
            get { return nameOfProducts; }
        }
        public double PriceOfProduct
        {
            set { }
            get { return priceOfProduct; }
        }
        public int CountOfProducts
        {
            set { }
            get { return countOfProducts; }
        }
        public string Comment
        {
            set { }
            get { return comment; }
        }
        public double PriceOfPurchase
        {
            set { }
            get { return priceOfPurchase; }
        }

        public Purchase(DateOnly date, string nameOfProducts, 
            double priceOfProduct, int countOfProducts, string comment)
        {
            this.date = date;
            this.nameOfProducts = nameOfProducts;
            this.priceOfProduct = priceOfProduct;
            this.countOfProducts = countOfProducts;
            this.comment = comment;

            priceOfPurchase = priceOfProduct * countOfProducts;
        }
    }
}
