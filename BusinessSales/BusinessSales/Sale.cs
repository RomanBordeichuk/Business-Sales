﻿namespace BusinessSales
{
    public class Sale
    {
        private int id;
        private DateTime date;
        private string nameOfProducts;
        private int priceOfProduct;
        private int countOfProducts;
        private string comment;

        private int priseOfSale;

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
        public int PriceOfProduct
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
        public int PriseOfSale
        {
            set { }
            get { return priseOfSale; }
        }

        public Sale(DateTime date, string nameOfProducts,
            int priceOfProduct, int countOfProducts, string comment)
        {
            this.date = date;
            this.nameOfProducts = nameOfProducts;
            this.priceOfProduct = priceOfProduct;
            this.countOfProducts = countOfProducts;
            this.comment = comment;
        }
    }
}
