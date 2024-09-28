using System.ComponentModel.DataAnnotations;

namespace CRUD_Demo.Models
{
    public class CustomerModel
    {
        public int? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string HomeAddress { get; set; }
        public string Email { get; set; }
        [MaxLength(10)]
        public string MobileNo { get; set; }
        [MaxLength(15)]
        public string GSTNo { get; set; }
        public string CityName { get; set; }
        [MaxLength(6)]
        public string PinCode { get; set; }
        public decimal NetAmount { get; set; }
        public int UserID { get; set; }
    }
}
