namespace BusinessSales
{
    record PurchaseJson(string date, string nameOfProducts, double priceOfProduct,
        int countOfProducts, string comment);

    public class Purchase
    {
        private int id;
        private DateTime date;
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
        public DateTime Date
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

        public Purchase(DateTime date, string nameOfProducts, 
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
