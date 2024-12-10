using CRUD_Demo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using ClosedXML.Excel;
using Irony.Parsing;

namespace CRUD_Demo.Controllers
{
    public class CustomerController : Controller
    {
        #region Configuration
        private IConfiguration configuration;
        public CustomerController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region GetCustomerData
        private DataTable GetCustomerData()
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Customer_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return table;
        }
        #endregion

        #region CustomerList
        public IActionResult CustomerList()
        {
            DataTable table = GetCustomerData();
            return View(table);
        }
        #endregion

        #region ExportToExcel
        public IActionResult ExportToExcel()
        {
            DataTable data = GetCustomerData();

            using (var workbook = new XLWorkbook())
            {
                //Console.WriteLine("workbook:- " + workbook);
                var worksheet = workbook.Worksheets.Add("Customer");
                //Console.WriteLine("worksheet:- " + worksheet);
                worksheet.Cell(1, 1).InsertTable(data);
                worksheet.Columns().AdjustToContents();  // Automatically adjust the column widths based on content

                using (var stream = new MemoryStream())
                {
                    //Console.WriteLine("stream:- " + stream);
                    workbook.SaveAs(stream);
                    stream.Position = 0;

                    string fileName = "Customer.xlsx";
                    Console.WriteLine(stream.ToArray());
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        #endregion

        #region CustomerDelete
        public IActionResult CustomerDelete(int CustomerID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Customer_DeleteByPK";
                command.Parameters.AddWithValue("@CustomerID", CustomerID);
                command.ExecuteNonQuery();
                TempData["SuccessMessage"] = "Deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "A foreign key constraint error occurred. Please try again.";
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("CustomerList");
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

        #region AddEditCustomer
        public IActionResult AddEditCustomer(int? CustomerID)
        {
            UserDropDown();
            CustomerModel customerModel = new CustomerModel();
            
            if(CustomerID != null)
            {
                TempData["IsEditMode"] = true;

                #region CustomerByID
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Customer_SelectByPK";
                command.Parameters.AddWithValue("@CustomerID ", CustomerID);
                SqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);

                foreach (DataRow dr in table.Rows)
                {
                    customerModel.CustomerName = @dr["CustomerName"].ToString();
                    customerModel.HomeAddress = @dr["HomeAddress"].ToString();
                    customerModel.Email = @dr["Email"].ToString();
                    customerModel.MobileNo = @dr["MobileNo"].ToString();
                    customerModel.GSTNo = @dr["GSTNo"].ToString();
                    customerModel.CityName = @dr["CityName"].ToString();
                    customerModel.PinCode = @dr["PinCode"].ToString();
                    customerModel.NetAmount = Convert.ToDecimal(@dr["NetAmount"]);
                    customerModel.UserID = Convert.ToInt32(@dr["UserID"]);
                }
                #endregion
            }
            else
            {
                TempData["IsEditMode"] = false;
            }
            TempData.Keep("IsEditMode");
            return View("AddEditCustomer", customerModel);
        }
        #endregion

        #region CustomerSave
        public IActionResult CustomerSave(CustomerModel customerModel)
        {
            ModelState.Remove("CustomerName");
            ModelState.Remove("HomeAddress");
            ModelState.Remove("Email");
            ModelState.Remove("MobileNo");
            ModelState.Remove("GSTNo");
            ModelState.Remove("CityName");
            ModelState.Remove("PinCode");
            ModelState.Remove("NetAmount");
            ModelState.Remove("UserID");

            if (string.IsNullOrEmpty(customerModel.CustomerName))
            {
                ModelState.AddModelError("CustomerName", "Please Enter Customer Name");
            }

            if (string.IsNullOrEmpty(customerModel.HomeAddress))
            {
                ModelState.AddModelError("HomeAddress", "Please Enter Home Address");
            }

            if (string.IsNullOrEmpty(customerModel.Email))
            {
                ModelState.AddModelError("Email", "Please Enter Valid Email");
            }

            if (string.IsNullOrEmpty(customerModel.MobileNo))
            {
                ModelState.AddModelError("MobileNo", "Please Enter Mobile Number");
            }

            if (string.IsNullOrEmpty(customerModel.GSTNo))
            {
                ModelState.AddModelError("GSTNo", "Please Enter GST Number");
            }

            if (string.IsNullOrEmpty(customerModel.CityName))
            {
                ModelState.AddModelError("CityName", "Please Enter City Name");
            }

            if (string.IsNullOrEmpty(customerModel.PinCode))
            {
                ModelState.AddModelError("PinCode", "Please Enter PinCode");
            }

            if (customerModel.NetAmount <= 0)
            {
                ModelState.AddModelError("NetAmount", "Please Enter Net Amount greater than 0");
            }

            if (customerModel.UserID <= 0)
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
                    if (customerModel.CustomerID == null)
                    {
                        command.CommandText = "PR_Customer_Insert";
                        command.Parameters.Add("@UserID", SqlDbType.Int).Value = customerModel.UserID;

                        TempData["SuccessMessage"] = "Customer added successfully!";
                    }
                    else
                    {
                        command.CommandText = "PR_Customer_UpdateByPK";
                        command.Parameters.Add("@customerID", SqlDbType.Int).Value = customerModel.CustomerID;

                        TempData["SuccessMessage"] = "Customer updated successfully!";
                    }
                    command.Parameters.Add("@CustomerName", SqlDbType.VarChar).Value = customerModel.CustomerName;
                    command.Parameters.Add("@HomeAddress", SqlDbType.VarChar).Value = customerModel.HomeAddress;
                    command.Parameters.Add("@Email", SqlDbType.VarChar).Value = customerModel.Email;
                    command.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = customerModel.MobileNo;
                    command.Parameters.Add("@GSTNo", SqlDbType.VarChar).Value = customerModel.GSTNo;
                    command.Parameters.Add("@CityName", SqlDbType.VarChar).Value = customerModel.CityName;
                    command.Parameters.Add("@PinCode", SqlDbType.VarChar).Value = customerModel.PinCode;
                    command.Parameters.Add("@NetAmount", SqlDbType.Decimal).Value = customerModel.NetAmount;

                    command.ExecuteNonQuery();
                    return RedirectToAction("AddEditCustomer", new { CustomerID = customerModel.CustomerID });
                }
            }
            UserDropDown();
            return View("AddEditCustomer", customerModel);
        }
        #endregion
    }
}
