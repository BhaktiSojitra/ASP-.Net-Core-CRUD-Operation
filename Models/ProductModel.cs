using System.ComponentModel.DataAnnotations;
namespace CRUD_Demo.Models
{
    public class ProductModel
    {
        public int? ProductID { get; set; }
        public string ProductName { get; set; } 
        public double ProductPrice { get; set; }
        [MaxLength(6)]
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public int UserID { get; set; }
    }

    public class UserDropDownModel 
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
}
