namespace PurchaseDetailTask.Models
{
    public class Sell
    {
        public int SellID { get; set; }

        public string InvoiceNo { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalDiscount { get; set; }

        public decimal TotalPurchase { get; set; }

        public decimal TotalProfit { get; set; }

        //public virtual IEnumerable<Details> Details { get; set; }
        public virtual List<Details> Details { get; set; } = new List<Details>();
    }
}