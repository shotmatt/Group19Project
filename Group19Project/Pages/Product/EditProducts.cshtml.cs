using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Group19Project.Models;
using Group19Project.Pages.DatabaseConnection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Group19Project.Pages.Product
{
    public class EditProductsModel : PageModel
    {
        [BindProperty]
        public Products Products { get; set; }
        public IActionResult OnGet(int? id)
        {
            DatabaseConnect dbstring = new DatabaseConnect();
            string DbConnection = dbstring.DatabaseString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            Products = new Products();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT * FROM Products WHERE ProductID = @PID";
                command.Parameters.AddWithValue("@PID", id);
                Console.WriteLine("Product ID : " + id);

                SqlDataReader reader = command.ExecuteReader();

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
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            Console.WriteLine("Product ID : " + Products.ProductID);
            Console.WriteLine("Product Name : " + Products.ProductName);
            Console.WriteLine("Product Price : " + Products.ProductPrice);
            Console.WriteLine("Product Category : " + Products.ProductCategory);
            Console.WriteLine("Product Description : " + Products.ProductDescription);

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "UPDATE Products SET ProductName=@PName, ProductPrice=@PPrice, ProductCategory=@PCategory, ProductDescription=@PDescription WHERE ProductID = @PID";
                command.Parameters.AddWithValue("@PID", Products.ProductID);
                command.Parameters.AddWithValue("@PName", Products.ProductName);
                command.Parameters.AddWithValue("@PPrice", Products.ProductPrice);
                command.Parameters.AddWithValue("@PCategory", Products.ProductCategory);
                command.Parameters.AddWithValue("@PDescription", Products.ProductDescription);


                command.ExecuteNonQuery();
            }
            conn.Close();
            return RedirectToPage("/Product/ProductIndex");
        }
    }
}