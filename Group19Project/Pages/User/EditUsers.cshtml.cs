using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Group19Project.Models;
using Group19Project.Pages.DatabaseConnection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Group19Project.Pages.User
{
    public class EditUsersModel : PageModel
    {
        [BindProperty]
        public Users Users { get; set; }
        public IActionResult OnGet(int? id)
        {
            DatabaseConnect dbstring = new DatabaseConnect();
            string DbConnection = dbstring.DatabaseString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            Users = new Users();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM Users WHERE UserID = @UID";

                command.Parameters.AddWithValue("@UID", id);
                Console.WriteLine("User ID : " + id);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {

                    Users.UserID = reader.GetInt32(0);
                    Users.FirstName = reader.GetString(1);
                    Users.LastName = reader.GetString(2);
                    Users.EmailAddress = reader.GetString(3);
                    Users.Password = reader.GetString(4);
                    Users.Role = reader.GetString(5);
                }

            }
            return Page();
        }

        public IActionResult OnPost()
        {
            DatabaseConnect dbstring = new DatabaseConnect();
            string DbConnection = dbstring.DatabaseString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            Console.WriteLine("User ID :" + Users.UserID);
            Console.WriteLine("User First Name : " + Users.FirstName);
            Console.WriteLine("User Last Name : " + Users.LastName);
            Console.WriteLine("User Email Address : " + Users.EmailAddress);
            Console.WriteLine("User Password : " + Users.Password);
            Console.WriteLine("User Role : " + Users.Role);


            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "UPDATE Users SET FirstName=@FName, LastName=@LName, EmailAddress=@EAddress, Password=@PWD, Role=@URole WHERE UserID = @UID";

                command.Parameters.AddWithValue("@UID", Users.UserID);
                command.Parameters.AddWithValue("@FName", Users.FirstName);
                command.Parameters.AddWithValue("@LName", Users.LastName);
                command.Parameters.AddWithValue("@EAddress", Users.EmailAddress);
                command.Parameters.AddWithValue("@PWD", Users.Password);
                command.Parameters.AddWithValue("@URole", Users.Role);

                command.ExecuteNonQuery();
            }
            conn.Close();
            return RedirectToPage("/User/UserManagement");
        }
    }
}