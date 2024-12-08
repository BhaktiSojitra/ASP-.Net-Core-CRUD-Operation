using CRUD_Demo.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using ClosedXML.Excel;

namespace CRUD_Demo.Controllers
{
    public class ProductController : Controller
    {
        #region Configuration
        private IConfiguration configuration;
        public ProductController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region GetProductData
        private DataTable GetProductData()
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Product_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return table;
        }
        #endregion

        #region ProductList
        public IActionResult ProductList()
        {
            DataTable table = GetProductData();
            return View(table);
        }
        #endregion

        #region ExportToExcel
        public IActionResult ExportToExcel()
        {
            DataTable data = GetProductData();

            using (var workbook = new XLWorkbook())
            {
                //Console.WriteLine("workbook:- " + workbook);
                var worksheet = workbook.Worksheets.Add("Product");
                //Console.WriteLine("worksheet:- " + worksheet);
                worksheet.Cell(1, 1).InsertTable(data);
                worksheet.Columns().AdjustToContents();  // Automatically adjust the column widths based on content

                using (var stream = new MemoryStream())
                {
                    //Console.WriteLine("stream:- " + stream);
                    workbook.SaveAs(stream);
                    stream.Position = 0;

                    string fileName = "Product.xlsx";
                    Console.WriteLine(stream.ToArray());
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        #endregion

        #region ProductDelete
        public IActionResult ProductDelete(int ProductID)
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            try
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Product_DeleteByPK";
                command.Parameters.AddWithValue("@ProductID", ProductID);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "A foreign key constraint error occurred. Please try again.";
                Console.WriteLine(ex.ToString());
            }
            return RedirectToAction("ProductList");
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

        #region AddEditProduct
        public IActionResult AddEditProduct(int? ProductID)
        {
            UserDropDown();
            ProductModel productModel = new ProductModel();

            if (ProductID != null && ProductID > 0)
            {
                TempData["IsEditMode"] = true;

                #region ProductByID
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Product_SelectByPK";
                command.Parameters.AddWithValue("@ProductID ", ProductID);
                SqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);

                foreach (DataRow dr in table.Rows)
                {
                    productModel.ProductName = @dr["ProductName"].ToString();
                    productModel.ProductPrice = Convert.ToDouble(@dr["ProductPrice"]);
                    productModel.ProductCode = @dr["ProductCode"].ToString();
                    productModel.Description = @dr["Description"].ToString();
                    productModel.UserID = Convert.ToInt32(@dr["UserID"]);
                }
                #endregion
            }
            else
            {
                TempData["IsEditMode"] = false;
            }
            TempData.Keep("IsEditMode");
            return View(productModel);
        }
        #endregion

        #region ProductSave
        public IActionResult ProductSave(ProductModel productModel)
        {
            ModelState.Remove("ProductName");
            ModelState.Remove("ProductPrice");
            ModelState.Remove("ProductCode");
            ModelState.Remove("Description");
            ModelState.Remove("UserID");

            if (string.IsNullOrEmpty(productModel.ProductName))
            {
                ModelState.AddModelError("ProductName", "Please Enter Product Name");
            }

            if (productModel.ProductPrice <= 0)
            {
                ModelState.AddModelError("ProductPrice", "Please Enter Product Price greater than 0");
            }

            if (string.IsNullOrEmpty(productModel.ProductCode))
            {
                ModelState.AddModelError("ProductCode", "Please Enter Product Code");
            }

            if (string.IsNullOrEmpty(productModel.Description))
            {
                ModelState.AddModelError("Description", "Please Enter Description");
            }

            if (productModel.UserID <= 0)
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
                    if (productModel.ProductID == null)
                    {
                        command.CommandText = "PR_Product_Insert";
                        command.Parameters.Add("@UserID", SqlDbType.Int).Value = productModel.UserID;

                        TempData["SuccessMessage"] = "Product added successfully!";
                    }
                    else
                    {
                        command.CommandText = "PR_Product_UpdateByPK";
                        command.Parameters.Add("@ProductID", SqlDbType.Int).Value = productModel.ProductID;

                        TempData["SuccessMessage"] = "Product updated successfully!";
                    }
                    command.Parameters.Add("@ProductName", SqlDbType.VarChar).Value = productModel.ProductName;
                    command.Parameters.Add("@ProductPrice", SqlDbType.Decimal).Value = productModel.ProductPrice;
                    command.Parameters.Add("@ProductCode", SqlDbType.VarChar).Value = productModel.ProductCode;
                    command.Parameters.Add("@Description", SqlDbType.VarChar).Value = productModel.Description;

                    command.ExecuteNonQuery();
                    return RedirectToAction("AddEditProduct", new { ProductID = productModel.ProductID });
                }
            }
            UserDropDown();
            return View("AddEditProduct", productModel);
        }
        #endregion
    }
}
