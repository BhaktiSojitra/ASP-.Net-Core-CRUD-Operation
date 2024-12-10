using CRUD_Demo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using ClosedXML.Excel;

namespace CRUD_Demo.Controllers
{
    public class OrderController : Controller
    {
        #region Configuration
        private IConfiguration configuration;
        public OrderController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region GetOrderData
        private DataTable GetOrderData()
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Order_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return table;
        }
        #endregion

        #region OrderList
        public IActionResult OrderList()
        {
            DataTable table = GetOrderData();
            return View(table);
        }
        #endregion

        #region ExportToExcel
        public IActionResult ExportToExcel()
        {
            DataTable data = GetOrderData();

            using (var workbook = new XLWorkbook())
            {
                //Console.WriteLine("workbook:- " + workbook);
                var worksheet = workbook.Worksheets.Add("Order");
                //Console.WriteLine("worksheet:- " + worksheet);
                worksheet.Cell(1, 1).InsertTable(data);
                worksheet.Columns().AdjustToContents();  // Automatically adjust the column widths based on content

                using (var stream = new MemoryStream())
                {
                    //Console.WriteLine("stream:- " + stream);
                    workbook.SaveAs(stream);
                    stream.Position = 0;

                    string fileName = "Order.xlsx";
                    Console.WriteLine(stream.ToArray());
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        #endregion

        #region OrderDelete
        public IActionResult OrderDelete(int OrderID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Order_DeleteByPK";
                command.Parameters.AddWithValue("@OrderID", OrderID);
                command.ExecuteNonQuery();
                TempData["SuccessMessage"] = "Deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "A foreign key constraint error occurred. Please try again.";
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("OrderList");
        }
        #endregion

        #region CustomerDropDown
        public void CustomerDropDown()
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand commandCustomer = connection.CreateCommand();
            commandCustomer.CommandType = CommandType.StoredProcedure;
            commandCustomer.CommandText = "PR_Customer_DropDown";
            SqlDataReader readerCustomer = commandCustomer.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(readerCustomer);
            readerCustomer.Close();

            List<CustomerDropDownModel> customerList = new List<CustomerDropDownModel>();
            foreach (DataRow dr in table.Rows)
            {
                CustomerDropDownModel model = new CustomerDropDownModel();
                model.CustomerID = Convert.ToInt32(dr["CustomerID"]);
                model.CustomerName = dr["CustomerName"].ToString();
                customerList.Add(model);
            }
            ViewBag.CustomerList = customerList;
        }
        #endregion

        #region UserDropDown
        public void UserDropDown()
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand commandUser = connection.CreateCommand();
            commandUser.CommandType = CommandType.StoredProcedure;
            commandUser.CommandText = "PR_User_DropDown";
            SqlDataReader readerUser = commandUser.ExecuteReader();
            DataTable table1 = new DataTable();
            table1.Load(readerUser);
            readerUser.Close();

            List<UserDropDownModel> userList = new List<UserDropDownModel>();
            foreach (DataRow dr in table1.Rows)
            {
                UserDropDownModel model = new UserDropDownModel();
                model.UserID = Convert.ToInt32(dr["UserID"]);
                model.UserName = dr["UserName"].ToString();
                userList.Add(model);
            }
            ViewBag.UserList = userList;
        }
        #endregion

        #region AddEditOrder
        public IActionResult AddEditOrder(int? OrderID)
        {
            CustomerDropDown();
            UserDropDown();
            OrderModel orderModel = new OrderModel();

            if(OrderID != null && OrderID > 0)
            {
                TempData["IsEditMode"] = true;

                #region OrderByID
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Order_SelectByPK";
                command.Parameters.AddWithValue("@OrderID", OrderID);
                SqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);

                foreach (DataRow dr in table.Rows)
                {
                    orderModel.OrderNumber = Convert.ToInt32(dr["OrderNumber"]);
                    orderModel.OrderDate = Convert.ToDateTime(dr["OrderDate"]);
                    orderModel.CustomerID = Convert.ToInt32(@dr["CustomerID"]);
                    orderModel.PaymentMode = @dr["PaymentMode"].ToString();
                    orderModel.TotalAmount = Convert.ToDecimal(@dr["TotalAmount"]);
                    orderModel.ShippingAddress = dr["ShippingAddress"].ToString();
                    orderModel.UserID = Convert.ToInt32(@dr["UserID"]);
                }
                #endregion
            }
            else
            {
                TempData["IsEditMode"] = false;
                orderModel.OrderDate = DateTime.Now;
            }
            TempData.Keep("IsEditMode");
            return View("AddEditOrder", orderModel);
        }
        #endregion

        #region OrderSave
        public IActionResult OrderSave(OrderModel orderModel)
        {
            ModelState.Remove("OrderNumber");
            ModelState.Remove("CustomerID");
            ModelState.Remove("PaymentMode");
            ModelState.Remove("TotalAmount");
            ModelState.Remove("ShippingAddress");
            ModelState.Remove("UserID");

            if (orderModel.OrderNumber <= 0)
            {
                ModelState.AddModelError("OrderNumber", "Please Enter Order Number greater than 0");
            }

            if (orderModel.CustomerID <= 0)
            {
                ModelState.AddModelError("CustomerID", "Please Select Customer");
            }

            if (string.IsNullOrEmpty(orderModel.PaymentMode))
            {
                ModelState.AddModelError("PaymentMode", "Please Enter Payment Mode");
            }

            if (!orderModel.TotalAmount.HasValue)
            {
                ModelState.AddModelError("TotalAmount", "Please Enter Total Amount greater than 0");
            }

            if (string.IsNullOrEmpty(orderModel.ShippingAddress))
            {
                ModelState.AddModelError("ShippingAddress", "Please Enter Shipping Address");
            }

            if (orderModel.UserID <= 0)
            {
                ModelState.AddModelError("UserID", "Please Select User");
            }

            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;

                    if (orderModel.OrderID == null)
                    {
                        command.CommandText = "PR_Order_Insert";
                        command.Parameters.Add("@OrderNumber", SqlDbType.Int).Value = orderModel.OrderNumber;
                        command.Parameters.Add("@CustomerID", SqlDbType.Int).Value = orderModel.CustomerID;
                        command.Parameters.Add("@UserID", SqlDbType.Int).Value = orderModel.UserID;

                        TempData["SuccessMessage"] = "Order added successfully!";
                    }
                    else
                    {
                        command.CommandText = "PR_Order_UpdateByPK";
                        command.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderModel.OrderID;

                        TempData["SuccessMessage"] = "Order updated successfully!";
                    }
                    command.Parameters.Add("@OrderDate", SqlDbType.DateTime).Value = orderModel.OrderDate;
                    command.Parameters.Add("@PaymentMode", SqlDbType.VarChar).Value = orderModel.PaymentMode;
                    command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = orderModel.TotalAmount;
                    command.Parameters.Add("@ShippingAddress", SqlDbType.VarChar).Value = orderModel.ShippingAddress;

                    command.ExecuteNonQuery();
                    return RedirectToAction("AddEditOrder", new { OrderID = orderModel.OrderID });
                }
            }

            CustomerDropDown();
            UserDropDown();
            return View("AddEditOrder", orderModel);
        }
        #endregion
    }
}
