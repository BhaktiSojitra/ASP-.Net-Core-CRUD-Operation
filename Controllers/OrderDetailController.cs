using CRUD_Demo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using ClosedXML.Excel;
using Irony.Parsing;

namespace CRUD_Demo.Controllers
{
    public class OrderDetailController : Controller
    {
        #region Configuration
        private IConfiguration configuration;
        public OrderDetailController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region GetOrderDetailData
        private DataTable GetOrderDetailData()
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_OrderDetail_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return table;
        }
        #endregion

        #region OrderDetailList
        public IActionResult OrderDetailList()
        {
            DataTable table = GetOrderDetailData();
            return View(table);
        }
        #endregion

        #region ExportToExcel
        public IActionResult ExportToExcel()
        {
            DataTable data = GetOrderDetailData();

            using (var workbook = new XLWorkbook())
            {
                //Console.WriteLine("workbook:- " + workbook);
                var worksheet = workbook.Worksheets.Add("OrderDetail");
                //Console.WriteLine("worksheet:- " + worksheet);
                worksheet.Cell(1, 1).InsertTable(data);
                worksheet.Columns().AdjustToContents();  // Automatically adjust the column widths based on content

                using (var stream = new MemoryStream())
                {
                    //Console.WriteLine("stream:- " + stream);
                    workbook.SaveAs(stream);
                    stream.Position = 0;

                    string fileName = "OrderDetail.xlsx";
                    Console.WriteLine(stream.ToArray());
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        #endregion

        #region OrderDetailDelete
        public IActionResult OrderDetailDelete(int OrderDetailID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_OrderDetail_DeleteByPK";
                command.Parameters.AddWithValue("@OrderDetailID", OrderDetailID);
                command.ExecuteNonQuery();
                TempData["SuccessMessage"] = "Deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "A foreign key constraint error occurred. Please try again.";
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("OrderDetailList");
        }
        #endregion

        #region OrderDropDown
        public void OrderDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_Order_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            connection1.Close();

            List<OrderDropDownModel> orders = new List<OrderDropDownModel>();
            foreach (DataRow dataRow in dataTable1.Rows)
            {
                OrderDropDownModel orderDropDownModel = new OrderDropDownModel();
                orderDropDownModel.OrderID = Convert.ToInt32(dataRow["OrderID"]);
                orderDropDownModel.OrderNumber = Convert.ToInt32(dataRow["OrderNumber"]);
                orders.Add(orderDropDownModel);
            }
            ViewBag.OrderList = orders;
        }
        #endregion

        #region ProductDropDown
        public void ProductDropDown()
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand commandProduct = connection.CreateCommand();
            commandProduct.CommandType = CommandType.StoredProcedure;
            commandProduct.CommandText = "PR_Product_DropDown";
            SqlDataReader readerCustomer = commandProduct.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(readerCustomer);
            readerCustomer.Close();

            List<ProductDropDownModel> ProductList = new List<ProductDropDownModel>();
            foreach (DataRow dr in table.Rows)
            {
                ProductDropDownModel model = new ProductDropDownModel();
                model.ProductID = Convert.ToInt32(dr["ProductID"]);
                model.ProductName = dr["ProductName"].ToString();
                ProductList.Add(model);
            }
            ViewBag.ProductList = ProductList;
        }
        #endregion

        #region UserDropDown
        public void UserDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection1 = new SqlConnection(connectionString);
            connection1.Open();
            SqlCommand command1 = connection1.CreateCommand();
            command1.CommandType = System.Data.CommandType.StoredProcedure;
            command1.CommandText = "PR_User_DropDown";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable dataTable1 = new DataTable();
            dataTable1.Load(reader1);
            connection1.Close();

            List<UserDropDownModel> users = new List<UserDropDownModel>();
            foreach (DataRow dataRow in dataTable1.Rows)
            {
                UserDropDownModel userDropDownModel = new UserDropDownModel();
                userDropDownModel.UserID = Convert.ToInt32(dataRow["UserID"]);
                userDropDownModel.UserName = dataRow["UserName"].ToString();
                users.Add(userDropDownModel);
            }
            ViewBag.UserList = users;
        }
        #endregion

        #region AddEditOrderDetail
        public IActionResult AddEditOrderDetail(int? OrderDetailID)
        {
            OrderDropDown();
            ProductDropDown();
            UserDropDown();
            OrderDetailModel orderDetailModel = new OrderDetailModel();

            if(OrderDetailID != null)
            {
                TempData["IsEditMode"] = true;

                #region OrderDetailByID
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_OrderDetail_SelectByPK";
                command.Parameters.AddWithValue("@OrderDetailID ", OrderDetailID);
                SqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);

                foreach (DataRow dr in table.Rows)
                {
                    orderDetailModel.OrderID = Convert.ToInt32(@dr["OrderID"]);
                    orderDetailModel.ProductID = Convert.ToInt32(@dr["ProductID"]);
                    orderDetailModel.Quantity = Convert.ToInt32(@dr["Quantity"]);
                    orderDetailModel.Amount = Convert.ToDecimal(@dr["Amount"]);
                    orderDetailModel.TotalAmount = Convert.ToDecimal(@dr["TotalAmount"]);
                    orderDetailModel.UserID = Convert.ToInt32(@dr["UserID"]);
                }
                #endregion
            }
            else
            {
                TempData["IsEditMode"] = false;
            }
            TempData.Keep("IsEditMode");
            return View("AddEditOrderDetail", orderDetailModel);
        }
        #endregion

        #region OrderDetailSave
        public IActionResult OrderDetailSave(OrderDetailModel orderDetailModel)
        {
            ModelState.Remove("OrderID");
            ModelState.Remove("ProductID");
            ModelState.Remove("Quantity");
            ModelState.Remove("Amount");
            ModelState.Remove("TotalAmount");
            ModelState.Remove("UserID");

            if (orderDetailModel.OrderID <= 0)
            {
                ModelState.AddModelError("OrderID", "Please Select Order");
            }

            if (orderDetailModel.ProductID <= 0)
            {
                ModelState.AddModelError("ProductID", "Please Select Product");
            }

            if (orderDetailModel.Quantity <= 0)
            {
                ModelState.AddModelError("Quantity", "Please Enter Quantity greater than 0");
            }

            if (orderDetailModel.Amount <= 0)
            {
                ModelState.AddModelError("Amount", "Please Enter Amount greater than 0");
            }

            if (orderDetailModel.TotalAmount <= 0)
            {
                ModelState.AddModelError("TotalAmount", "Please Enter Total Amount greater than 0");
            }

            if (orderDetailModel.UserID <= 0)
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
                    if (orderDetailModel.OrderDetailID == null)
                    {
                        command.CommandText = "PR_OrderDetail_Insert";
                        command.Parameters.Add("@OrderID", SqlDbType.Int).Value = orderDetailModel.OrderID;
                        command.Parameters.Add("@ProductID", SqlDbType.Int).Value = orderDetailModel.ProductID;
                        command.Parameters.Add("@UserID", SqlDbType.Int).Value = orderDetailModel.UserID;

                        TempData["SuccessMessage"] = "Order Detail added successfully!";
                    }
                    else
                    {
                        command.CommandText = "PR_OrderDetail_UpdateByPK";
                        command.Parameters.Add("@OrderDetailID", SqlDbType.Int).Value = orderDetailModel.OrderDetailID;

                        TempData["SuccessMessage"] = "Order Detail updated successfully!";
                    }
                    command.Parameters.Add("@Quantity", SqlDbType.Int).Value = orderDetailModel.Quantity;
                    command.Parameters.Add("@Amount", SqlDbType.Decimal).Value = orderDetailModel.Amount;
                    command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = orderDetailModel.TotalAmount;

                    command.ExecuteNonQuery();
                    return RedirectToAction("AddEditOrderDetail", new { OrderDetailID = orderDetailModel.OrderDetailID });
                }
            }
            OrderDropDown();
            ProductDropDown();
            UserDropDown();
            return View("AddEditOrderDetail", orderDetailModel);
        }
        #endregion
    }
}
