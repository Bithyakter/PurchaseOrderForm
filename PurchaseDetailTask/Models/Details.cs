using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PurchaseDetailTask.Models
{
    public class Details
    {
        [Key]
        public int DetailsId { get; set; }

        public string Product { get; set; }

        public int Quantity { get; set; }

        public decimal PurchasePrice { get; set; }

        public decimal SellPrice { get; set; }

        public decimal Discount { get; set; }

        public decimal Total { get; set; }

        public int SellID { get; set; }

        [ForeignKey("SellID")]
        public virtual Sell Sells { get; set; }
    }
}