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

namespace Group19Project.Pages.Login
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Users Users { get; set; }


        public string Message { get; set; }
        public string SessionID;
        public void OnGet()
        {

        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            DatabaseConnect dbstring = new DatabaseConnect();
            string DbConnection = dbstring.DatabaseString();
            Console.WriteLine(DbConnection);
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            Console.WriteLine(Users.EmailAddress);
            Console.WriteLine(Users.Password);

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"SELECT u.UserID, u.FirstName, u.LastName, u.EmailAddress, u.Password, u.Role FROM Users AS u WHERE u.EmailAddress = @EAddress AND u.Password = @Pwd";

                command.Parameters.AddWithValue("@EAddress", Users.EmailAddress);
                command.Parameters.AddWithValue("@Pwd", Users.Password);

                var reader = command.ExecuteReader();

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

            if (!string.IsNullOrEmpty(Users.FirstName))
            {
                SessionID = HttpContext.Session.Id;
                HttpContext.Session.SetString("sessionID", SessionID);
                HttpContext.Session.SetString("FirstName", Users.FirstName);
                HttpContext.Session.SetString("EmailAddress", Users.EmailAddress);


                if (Users.Role == "Customer")
                {
                    return RedirectToPage("/UserPages/UserIndex");
                }
                else
                {
                    return RedirectToPage("/AdminPages/AdminIndex");
                }


            }
            else
            {
                Message = "Invalid Email Address and Password!";
                return Page();
            }



        }
    }
}
