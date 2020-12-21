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

namespace Group19Project.Pages.AdminPages
{
    public class ViewUsersModel : PageModel
    {
        [BindProperty]
        public List<Users> Users { get; set; }




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
                command.CommandText = @"SELECT * FROM Users ORDER BY UserID ASC";

                var reader = command.ExecuteReader();

                Users = new List<Users>();
                while (reader.Read())
                {
                    Users Row = new Users();
                    Row.UserID = reader.GetInt32(0);
                    Row.FirstName = reader.GetString(1);
                    Row.LastName = reader.GetString(2);
                    Row.EmailAddress = reader.GetString(3);
                    Row.Password = reader.GetString(4);
                    Row.Role = reader.GetString(5);
                    Users.Add(Row);
                }

            }
            return Page();

        }
    }
}