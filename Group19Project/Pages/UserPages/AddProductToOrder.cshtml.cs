using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Group19Project.Models;
using Group19Project.Pages.DatabaseConnection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Group19Project.Pages.UserPages
{
    public class AddProductToOrderModel : PageModel
    {
        [BindProperty]
        public Products Products { get; set; }

        [BindProperty]
        public Orders Orders { get; set; }

        public List<string> URole { get; set; } = new List<string> { "User", "Admin" };
        public string EmailAddress;
        public const string SessionKeyName1 = "EmailAddress";


        public string FirstName;
        public const string SessionKeyName2 = "FirstName";

        public string SessionID;
        public const string SessionKeyName3 = "sessionID";

        public IActionResult OnGet(int? id)
        {

            EmailAddress = HttpContext.Session.GetString(SessionKeyName1);
            FirstName = HttpContext.Session.GetString(SessionKeyName2);
            SessionID = HttpContext.Session.GetString(SessionKeyName3);

            DatabaseConnect dbstring = new DatabaseConnect();
            string DbConnection = dbstring.DatabaseString();
            Console.WriteLine(DbConnection);
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM Products WHERE ProductID = @PID";
                command.Parameters.AddWithValue("@PID", id);

                SqlDataReader reader = command.ExecuteReader();
                Products = new Products();
                while (reader.Read())
                {
                    Products.ProductID = reader.GetInt32(0);
                    Products.ProductName = reader.GetString(1);
                    Products.ProductPrice = reader.GetString(2);
                    Products.ProductCategory = reader.GetString(3);
                    Products.ProductDescription = reader.GetString(4);
                }

            }

            conn.Close();

            return Page();
        }

        public IActionResult OnPost()
        {
            DatabaseConnect dbstring = new DatabaseConnect();
            string DbConnection = dbstring.DatabaseString();
            Console.WriteLine(DbConnection);
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "INSERT INTO Orders (ProductID, ProductName) SELECT ProductID, ProductName FROM Products WHERE ProductID = @PID";
                command.Parameters.AddWithValue("@PID", Products.ProductID);
                command.ExecuteNonQuery();
            }

            conn.Close();
            return RedirectToPage("/UserPages/UserIndex");
        }

    }
}
