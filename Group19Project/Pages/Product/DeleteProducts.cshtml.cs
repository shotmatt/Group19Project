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
    public class DeleteProductsModel : PageModel
    {
        [BindProperty]
        public List<Products> Prod { get; set; }
        [BindProperty]
        public List<bool> IsSelect { get; set; }

        public List<Products> ProdToDelete { get; set; }

        public IActionResult OnGet()
        {
            DatabaseConnect dbstring = new DatabaseConnect();
            string DbConnection = dbstring.DatabaseString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM Products";

                SqlDataReader reader = command.ExecuteReader();

                Prod = new List<Products>();
                IsSelect = new List<bool>();
                while (reader.Read())
                {
                    Products rec = new Products();
                    rec.ProductID = reader.GetInt32(0);
                    rec.ProductName = reader.GetString(1);
                    rec.ProductPrice = reader.GetString(2);
                    rec.ProductCategory = reader.GetString(3);
                    rec.ProductDescription = reader.GetString(4);
                    Prod.Add(rec);
                    IsSelect.Add(false);

                }
            }


            return Page();

        }

        public IActionResult OnPost()
        {
            ProdToDelete = new List<Products>();
            for (int i = 0; i < Prod.Count; i++)
            {
                if (IsSelect[i] == true)
                {
                    ProdToDelete.Add(Prod[i]);
                }
            }

            Console.WriteLine("Product to be deleted : ");

            for (int i = 0; i < ProdToDelete.Count(); i++)
            {
                Console.WriteLine(ProdToDelete[i].ProductID);
                Console.WriteLine(ProdToDelete[i].ProductName);
                Console.WriteLine(ProdToDelete[i].ProductPrice);
                Console.WriteLine(ProdToDelete[i].ProductCategory);
                Console.WriteLine(ProdToDelete[i].ProductDescription);


            }

            DatabaseConnect dbstring = new DatabaseConnect();
            string DbConnection = dbstring.DatabaseString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            for (int i = 0; i < ProdToDelete.Count(); i++)
            {

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText = @"DELETE FROM Products WHERE ProductID = @PID";
                    command.Parameters.AddWithValue("@PID", ProdToDelete[i].ProductID);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToPage("/Product/ProductIndex");


        }

    }
}
