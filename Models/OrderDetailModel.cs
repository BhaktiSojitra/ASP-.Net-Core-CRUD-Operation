using System.ComponentModel.DataAnnotations;
namespace CRUD_Demo.Models
{
    public class OrderDetailModel
    {
        public int? OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public int UserID { get; set; }
    }

    public class ProductDropDownModel 
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
    }

    public class OrderDropDownModel
    {
        public int OrderID { get; set; }
        public int OrderNumber { get; set; }
    }
}
