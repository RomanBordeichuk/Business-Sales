namespace BusinessSales
{
    public class Account
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Password { set; get; }

        public List<Purchase> Purchases { set; get; }
        public List<Sale> Sales { set; get; }
        public List<ProductsBatch> Store { set; get; }

        public Account(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}
