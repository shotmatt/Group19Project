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

namespace Group19Project.Pages.User
{
    public class CreateUserModel : PageModel
    {
        [BindProperty]
        public Users Users { get; set; }

        public List<string> URole { get; set; } = new List<string> { "Customer", "Admin" };
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

            return RedirectToPage("/User/UserManagement");
        }
    }
}