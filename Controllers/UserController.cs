using CRUD_Demo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using ClosedXML.Excel;

namespace CRUD_Demo.Controllers
{
    public class UserController : Controller
    {
        #region Configuration
        private IConfiguration configuration;
        public UserController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        //List<UserModel> users = new List<UserModel>()
        //{
        //    new UserModel {UserID = 1 , UserName = "xyz" , Email = "xyz@gmail.com" , Password = "101" , MobileNo = "1234567890" , Address = "line1" , IsActive = false},
        //    new UserModel {UserID = 2 , UserName = "abc" , Email = "abc@gmail.com" , Password = "102" , MobileNo = "0123456789" , Address = "line2" , IsActive = true }
        //};

        #region GetUserData
        private DataTable GetUserData()
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_User_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return table;
        }
        #endregion

        #region UserList
        public IActionResult UserList()
        {
            DataTable table = GetUserData();
            return View(table);
        }
        #endregion

        #region ExportToExcel
        public IActionResult ExportToExcel()
        {
            DataTable data = GetUserData();

            using (var workbook = new XLWorkbook())
            {
                //Console.WriteLine("workbook:- " + workbook);
                var worksheet = workbook.Worksheets.Add("User");
                //Console.WriteLine("worksheet:- " + worksheet);
                worksheet.Cell(1, 1).InsertTable(data);
                worksheet.Columns().AdjustToContents();  // Automatically adjust the column widths based on content

                using (var stream = new MemoryStream())
                {
                    //Console.WriteLine("stream:- " + stream);
                    workbook.SaveAs(stream);
                    stream.Position = 0;

                    string fileName = "User.xlsx";
                    Console.WriteLine(stream.ToArray());
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        #endregion

        #region UserDelete
        public IActionResult UserDelete(int UserID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_User_DeleteByPK";
                command.Parameters.AddWithValue("@UserID", UserID);
                command.ExecuteNonQuery();
                TempData["SuccessMessage"] = "Deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.ToString();
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("UserList");
        }
        #endregion

        #region AddEditUser
        public IActionResult AddEditUser(int? UserID)
        {
            UserModel userModel = new UserModel();

            if (UserID != null)
            {
                TempData["IsEditMode"] = true;

                #region UserByID
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_User_SelectByPK";
                command.Parameters.AddWithValue("@UserID", UserID);
                SqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);

                foreach (DataRow dr in table.Rows)
                {
                    userModel.UserName = @dr["UserName"].ToString();
                    userModel.Email = @dr["Email"].ToString();
                    userModel.Password = @dr["Password"].ToString();
                    userModel.MobileNo = @dr["MobileNo"].ToString();
                    userModel.Address = @dr["Address"].ToString();
                    userModel.IsActive = Convert.ToBoolean(@dr["IsActive"]);
                }
                #endregion
            }
            else
            {
                TempData["IsEditMode"] = false;
            }
            TempData.Keep("IsEditMode");
            return View("AddEditUser", userModel);
        }
        #endregion

        #region UserSave
        public IActionResult UserSave(UserModel userModel)
        {
            ModelState.Remove("UserName");
            ModelState.Remove("Email");
            ModelState.Remove("Password");
            ModelState.Remove("MobileNo");
            ModelState.Remove("Address");
            ModelState.Remove("IsActive");

            if (string.IsNullOrEmpty(userModel.UserName))
            {
                ModelState.AddModelError("UserName", "Please Enter UserName");
            }

            if (string.IsNullOrEmpty(userModel.Email))
            {
                ModelState.AddModelError("Email", "Please Enter Email Address.");
            }

            if (string.IsNullOrEmpty(userModel.Password))
            {
                ModelState.AddModelError("Password", "Please Enter Password");
            }

            if (string.IsNullOrEmpty(userModel.MobileNo))
            {
                ModelState.AddModelError("MobileNo", "Please Enter MobileNo");
            }

            if (string.IsNullOrEmpty(userModel.Address))
            {
                ModelState.AddModelError("Address", "Please Enter Address");
            }
            
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    if (userModel.UserID == null)
                    {
                        command.CommandText = "PR_User_Insert";
                        TempData["SuccessMessage"] = "User added successfully!";
                    }
                    else
                    {
                        command.CommandText = "PR_User_UpdateByPK";
                        command.Parameters.Add("@UserID", SqlDbType.Int).Value = userModel.UserID;

                        TempData["SuccessMessage"] = "User updated successfully!";
                    }
                    command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userModel.UserName;
                    command.Parameters.Add("@Email", SqlDbType.VarChar).Value = userModel.Email;
                    command.Parameters.Add("@Password", SqlDbType.VarChar).Value = userModel.Password;
                    command.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = userModel.MobileNo;
                    command.Parameters.Add("@Address", SqlDbType.VarChar).Value = userModel.Address;
                    command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = userModel.IsActive;
                    command.ExecuteNonQuery();
                    return RedirectToAction("AddEditUser", new { UserID = userModel.UserID });
                }
            }
            return View("AddEditUser", userModel);
        }
        #endregion

        #region UserLogin
        public IActionResult UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this.configuration.GetConnectionString("ConnectionString");
                    SqlConnection sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_User_Login";
                    sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userLoginModel.UserName;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userLoginModel.Password;
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(sqlDataReader);
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                            HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                        }
                        return RedirectToAction("ProductList", "Product");
                    }
                    else
                    {
                        return RedirectToAction("Login", "User");
                    }

                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }

            return RedirectToAction("Login");
        }
        #endregion

        #region Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }
        #endregion

        #region Login
        public IActionResult Login()
        {
            return View(new UserLoginModel());
        }
        #endregion

        #region UserRegister
        public IActionResult UserRegister(UserRegisterModel userRegisterModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this.configuration.GetConnectionString("ConnectionString");
                    SqlConnection sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_User_Register";
                    sqlCommand.Parameters.Add("@UserName", SqlDbType.VarChar).Value = userRegisterModel.UserName;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userRegisterModel.Password;
                    sqlCommand.Parameters.Add("@Email", SqlDbType.VarChar).Value = userRegisterModel.Email;
                    sqlCommand.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = userRegisterModel.MobileNo;
                    sqlCommand.Parameters.Add("@Address", SqlDbType.VarChar).Value = userRegisterModel.Address;
                    sqlCommand.ExecuteNonQuery();
                    return RedirectToAction("Login", "User");
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToAction("Register");
            }
            return RedirectToAction("Register");
        }
        #endregion

        #region Register
        public IActionResult Register()
        {
            return View("Register");
        }
        #endregion
    }
}
