namespace BusinessSales
{
    record SaleJson(string date, string nameOfProducts, 
        string priceOfProduct, string countOfProducts, string comment);
    record SaleResponseJson(string date, string nameOfProducts, 
        string priceOfProduct, string countOfProducts, 
        string comments, string priceOfSale, string costOfSale);

    public class Sale
    {
        private int id;
        private DateOnly date;
        private string nameOfProducts;
        private double priceOfProduct;
        private int countOfProducts;
        private string comment;

        private double priceOfSale;

        private double costOfSale;

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
        public double PriceOfSale
        {
            set { }
            get { return priceOfSale; }
        }
        public double CostOfSale
        {
            set { }
            get { return costOfSale; }
        }

        public Sale(DateOnly date, string nameOfProducts,
            double priceOfProduct, int countOfProducts, 
            string comment, double costOfSale)
        {
            this.date = date;
            this.nameOfProducts = nameOfProducts;
            this.priceOfProduct = priceOfProduct;
            this.countOfProducts = countOfProducts;
            this.comment = comment;

            priceOfSale = priceOfProduct * countOfProducts;

            this.costOfSale = costOfSale;
        }
    }
}
