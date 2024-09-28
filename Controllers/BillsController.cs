using CRUD_Demo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using ClosedXML.Excel;
using Irony.Parsing;

namespace CRUD_Demo.Controllers
{
    public class BillsController : Controller
    {
        #region Configuration
        private IConfiguration configuration;
        public BillsController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region GetBillsData
        private DataTable GetBillsData()
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Bills_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return table;
        }
        #endregion

        #region BillsList
        public IActionResult BillsList()
        {
            DataTable table = GetBillsData();
            return View(table);
        }
        #endregion

        #region ExportToExcel
        public IActionResult ExportToExcel()
        {
            DataTable data = GetBillsData();

            using (var workbook = new XLWorkbook())
            {
                //Console.WriteLine("workbook:- " + workbook);
                var worksheet = workbook.Worksheets.Add("Bills");
                //Console.WriteLine("worksheet:- " + worksheet);
                worksheet.Cell(1, 1).InsertTable(data);
                worksheet.Columns().AdjustToContents();  // Automatically adjust the column widths based on content

                using (var stream = new MemoryStream())
                {
                    //Console.WriteLine("stream:- " + stream);
                    workbook.SaveAs(stream);
                    stream.Position = 0;

                    string fileName = "Bills.xlsx";
                    Console.WriteLine(stream.ToArray());
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        #endregion

        #region BillDelete
        public IActionResult BillDelete(int BillID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Bills_DeleteByPK";
                command.Parameters.AddWithValue("@BillID", BillID);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "A foreign key constraint error occurred. Please try again.";
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("BillsList");
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

        #region AddEditBills
        public IActionResult AddEditBills(int? BillID)
        {
            OrderDropDown();
            UserDropDown();
            BillsModel billsModel = new BillsModel();

            if(BillID != null)
            {
                ViewBag.IsEditMode = true;

                #region BillsByID
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Bills_SelectByPK";
                command.Parameters.AddWithValue("@BillID ", BillID);
                SqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);

                foreach (DataRow dr in table.Rows)
                {
                    billsModel.BillNumber = @dr["BillNumber"].ToString();
                    billsModel.BillDate = Convert.ToDateTime(@dr["BillDate"]);
                    billsModel.OrderID = Convert.ToInt32(@dr["OrderID"]);
                    billsModel.TotalAmount = Convert.ToDecimal(@dr["TotalAmount"]);
                    billsModel.Discount = Convert.ToDecimal(@dr["Discount"]);
                    billsModel.NetAmount = Convert.ToDecimal(@dr["NetAmount"]);
                    billsModel.UserID = Convert.ToInt32(@dr["UserID"]);
                }
                #endregion
            }
            else
            {
                ViewBag.IsEditMode = false;
            }
            return View("AddEditBills", billsModel);
        }
        #endregion

        #region BillsSave
        public IActionResult BillsSave(BillsModel billsModel)
        {
            ModelState.Remove("BillNumber");
            ModelState.Remove("BillDate");
            ModelState.Remove("OrderID");
            ModelState.Remove("TotalAmount");
            ModelState.Remove("Discount");
            ModelState.Remove("NetAmount");
            ModelState.Remove("UserID");

            if (string.IsNullOrEmpty(billsModel.BillNumber))
            {
                ModelState.AddModelError("BillNumber", "Please Enter Bill Number");
            }

            if (billsModel.BillDate < DateTime.Now.Date)
            {
                ModelState.AddModelError("BillDate", "Please Enter Today Date and Time");
            }

            if (billsModel.OrderID <= 0)
            {
                ModelState.AddModelError("OrderID", "Please Select Order");
            }

            if (billsModel.TotalAmount <= 0)
            {
                ModelState.AddModelError("TotalAmount", "Please Enter Total Amount greater than 0");
            }

            if (!billsModel.Discount.HasValue)
            {
                ModelState.AddModelError("Discount", "Please Enter Discount Value");
            }

            if (billsModel.NetAmount <= 0)
            {
                ModelState.AddModelError("NetAmount", "Please Enter Net Amount greater than 0");
            }

            if (billsModel.UserID <= 0)
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
                    if (billsModel.BillID == null)
                    {
                        command.CommandText = "PR_Product_Insert";
                        command.Parameters.Add("@BillNumber", SqlDbType.VarChar).Value = billsModel.BillNumber;
                        command.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = billsModel.BillDate;
                        command.Parameters.Add("@OrderID", SqlDbType.Int).Value = billsModel.OrderID;
                        command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = billsModel.TotalAmount;
                        command.Parameters.Add("@Discount", SqlDbType.Decimal).Value = billsModel.Discount;
                        command.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = billsModel.NetAmount;
                        command.Parameters.Add("@UserID", SqlDbType.Int).Value = billsModel.UserID;

                        TempData["SuccessMessage"] = "Bill added successfully!";
                    }
                    else
                    {
                        command.CommandText = "PR_Bills_UpdateByPK";
                        command.Parameters.Add("@BillID", SqlDbType.Int).Value = billsModel.BillID;
                        command.Parameters.Add("@BillNumber", SqlDbType.VarChar).Value = billsModel.BillNumber;
                        command.Parameters.Add("@BillDate", SqlDbType.DateTime).Value = billsModel.BillDate;
                        command.Parameters.Add("@TotalAmount", SqlDbType.Decimal).Value = billsModel.TotalAmount;
                        command.Parameters.Add("@Discount", SqlDbType.Decimal).Value = billsModel.Discount;
                        command.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = billsModel.NetAmount;

                        TempData["SuccessMessage"] = "Bill updated successfully!";
                    }

                    command.ExecuteNonQuery();
                    return RedirectToAction("AddEditBills", new { BillID = billsModel.BillID });
                }
            }
            OrderDropDown();
            UserDropDown();
            return View("AddEditBills", billsModel);
        }
        #endregion
    }
}
