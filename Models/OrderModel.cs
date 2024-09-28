using System.ComponentModel.DataAnnotations;
namespace CRUD_Demo.Models
{
    public class OrderModel
    {
        public int? OrderID { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM.DD.YYYY}")]
        public DateTime OrderDate { get; set; }

        public int OrderNumber { get; set; }

        public int CustomerID { get; set; }

        [DataType(DataType.CreditCard)]
        public string? PaymentMode { get; set; }

        public decimal? TotalAmount { get; set; }

        public string ShippingAddress { get; set; }

        public int UserID { get; set; }
    }

    public class CustomerDropDownModel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
    }
}
