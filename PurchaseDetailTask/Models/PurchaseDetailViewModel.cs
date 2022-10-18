namespace PurchaseDetailTask.Models
{
    public class PurchaseDetailViewModel
    {
        public int SellID { get; set; }

        public string? InvoiceNo { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalDiscount { get; set; }

        public decimal TotalPurchase { get; set; }

        public decimal TotalProfit { get; set; }

        public List<Details> details { get; set; }
    }
}
