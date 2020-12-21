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

namespace Group19Project.Pages.Product
{
    public class ViewProductsModel : PageModel
    {
        [BindProperty]
        public List<Users> Users { get; set; }
        public List<Products> Products { get; set; }

        public List<string> URole { get; set; } = new List<string> { "User", "Admin" };
        public string EmailAddress;
        public const string SessionKeyName1 = "EmailAddress";


        public string FirstName;
        public const string SessionKeyName2 = "FirstName";

        public string SessionID;
        public const string SessionKeyName3 = "sessionID";
        public IActionResult OnGet()
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
                command.CommandText = @"SELECT * FROM Products ORDER BY ProductID ASC";

                var reader = command.ExecuteReader();

                Products = new List<Products>();
                while (reader.Read())
                {
                    Products Row = new Products();
                    Row.ProductID = reader.GetInt32(0);
                    Row.ProductName = reader.GetString(1);
                    Row.ProductPrice = reader.GetString(2);
                    Row.ProductCategory = reader.GetString(3);
                    Row.ProductDescription = reader.GetString(4);

                    Products.Add(Row);
                }

            }
            return Page();

        }
    }
}