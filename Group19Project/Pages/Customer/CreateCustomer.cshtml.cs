using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Group19Project.Models;
using Group19Project.Pages.DatabaseConnection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Group19Project.Pages.Customer
{
    public class CreateCustomerModel : PageModel
    {
        [BindProperty]
        public Users Users { get; set; }

        public List<string> URole { get; set; } = new List<string> { "Customer" };




        public IActionResult OnPost()
        {
            DatabaseConnect dbstring = new DatabaseConnect();
            string DbConnection = dbstring.DatabaseString();
            Console.WriteLine(DbConnection);
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();


            Console.WriteLine(Users.FirstName);
            Console.WriteLine(Users.LastName);
            Console.WriteLine(Users.EmailAddress);
            Console.WriteLine(Users.Password);
            Console.WriteLine(Users.Role);


            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"INSERT INTO Users (FirstName, LastName, EmailAddress, Password, Role) VALUES (@FName, @LName, @EAddress, @Pwd, @Role)";


                command.Parameters.AddWithValue("@FName", Users.FirstName);
                command.Parameters.AddWithValue("@LName", Users.LastName);
                command.Parameters.AddWithValue("@EAddress", Users.EmailAddress);
                command.Parameters.AddWithValue("@Pwd", Users.Password);
                command.Parameters.AddWithValue("@Role", Users.Role);
                command.ExecuteNonQuery();
            }

            return RedirectToPage("/Login/Login");
        }
    }
}