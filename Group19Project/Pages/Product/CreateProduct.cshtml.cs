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
    public class CreateProductModel : PageModel
    {
        [BindProperty]
        public Products Products { get; set; }


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

            if (string.IsNullOrEmpty(EmailAddress) && string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(SessionID))
            {
                return RedirectToPage("/Login/Login");
            }
            return Page();


        }

        public IActionResult OnPost()
        {
            DatabaseConnect dbstring = new DatabaseConnect();
            string DbConnection = dbstring.DatabaseString();
            Console.WriteLine(DbConnection);
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            Console.WriteLine(Products.ProductName);
            Console.WriteLine(Products.ProductPrice);
            Console.WriteLine(Products.ProductCategory);
            Console.WriteLine(Products.ProductDescription);

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "INSERT INTO Products (ProductName, ProductPrice, ProductCategory, ProductDescription) VALUES (@PName, @PPrice, @PCategory, @PDescription)";


                command.Parameters.AddWithValue("@PName", Products.ProductName);
                command.Parameters.AddWithValue("@PPrice", Products.ProductPrice);
                command.Parameters.AddWithValue("@PCategory", Products.ProductCategory);
                command.Parameters.AddWithValue("@PDescription", Products.ProductDescription);
                command.ExecuteNonQuery();
            }

            return RedirectToPage("/Product/ProductIndex");
        }
    }
}
