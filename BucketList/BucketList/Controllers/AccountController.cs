using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BucketList.Models;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Configuration;




namespace BucketList.Controllers
{
    public class AccountController : Controller
    {      
        
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        //Login
       [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User model, string returnUrl)
        {
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(Request.Form["password"], System.Web.Configuration.FormsAuthPasswordFormat.SHA1.ToString());
            bool check = CheckPassword(Request.Form["username"], password);
            if (check)
            {
                FormsAuthentication.SetAuthCookie(Request.Form["username"], true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Incorrect password or username.";
                return View("Login");
            }
        }
        public bool CheckPassword(string Username, string Password)
        {
            bool check = false;
            String CS = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            SqlConnection connection = new SqlConnection(CS);
            string cmd = "Select count(*) from [User] where [username]='" + Username + "'and [password]='" + Password + "'";
            connection.Open();
            SqlCommand command = new SqlCommand(cmd, connection);
            check = Convert.ToBoolean(command.ExecuteScalar());
            connection.Close();
            return check;
        }
        public bool CheckUsername(string Username)
        {
            bool check = false;
            String CS = ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            SqlConnection connection = new SqlConnection(CS);
            string cmd = "Select count(*) from [User] where [username]='" + Username + "'";
            connection.Open();
            SqlCommand command = new SqlCommand(cmd, connection);
            check = Convert.ToBoolean(command.ExecuteScalar());
            connection.Close();
            return check;
        }
        //Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        //signup
        [AllowAnonymous]
        public ActionResult SignUp()
        {
            return View();
        }
        //maintain account
        [Authorize]
        public ActionResult MaintainAccount()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            int userID = GetUserID(ticket.Name);
            //var user = Database.Users.Where(s => s.Id == userID).FirstOrDefault();
            // return View(user);
            return View();
        }

        [HttpPost]
        public ActionResult UpdateInfo([Bind(Include = "Id, Username, Password, Type, Name, Surname, Email")]User userToUpdate)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
               // Data.Entry(userToUpdate).State = EntityState.Modified;
                //Data.SaveChanges();
                TempData["SuccessMessage"] = "Your information has been updated!";
                return RedirectToAction("Index");
            }
            return View(userToUpdate);

        }

        public int GetUserID(string name)
        {
            String CS = ConfigurationManager.ConnectionStrings["DataModel"].ConnectionString;
            SqlConnection connection = new SqlConnection(CS);
            string cmd = "Select [Id] from [User] where [Username]='" + name + "'";
            connection.Open();
            SqlCommand command = new SqlCommand(cmd, connection);
            SqlDataReader reader = command.ExecuteReader();
            int role = 0;
            while (reader.Read())
                role = reader.GetInt32(0);
            connection.Close();
            reader.Close();
            return role;
        }

        public ActionResult ViewOwnProfile()
        {

            return View();

        }
    }
}