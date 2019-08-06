using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Blush.Models;

namespace Blush.Controllers
{
    public class SecurityController : Controller
    {
        DataModel Data = new DataModel();

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CheckUser()
        {
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(Request.Form["Password"], System.Web.Configuration.FormsAuthPasswordFormat.SHA1.ToString());
            bool check = CheckPassword(Request.Form["Username"], password);
            if (check)
            {
                FormsAuthentication.SetAuthCookie(Request.Form["Username"], true);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Incorrect password or username.";
                return View("Login");
            }
        }
        [AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public ActionResult RegisterUser()
        {
            return View("RegisterUser");
        }

        [HttpPost]
        public ActionResult RegisterUser([Bind(Exclude = "Id, Type")] User userToAdd)
        {
            try
            {
                // TODO: Add insert logic here 
                bool check = CheckUsername(userToAdd.Username);
                if (check)
                {
                    ///NEED TO ADD EXCEPTION HERE!!!!
                    TempData["ErrorMessage"] = "Chosen username is already in use.";
                    return View("RegisterUser");
                }
                else
                {
                    userToAdd.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(userToAdd.Password, System.Web.Configuration.FormsAuthPasswordFormat.SHA1.ToString());
                    userToAdd.Type = "User";
                    Data.Users.Add(userToAdd);
                    Data.SaveChanges();
                    FormsAuthentication.SetAuthCookie(Request.Form["Username"], true);
                    return RedirectToAction("Index", "Home");
                }
            }
            catch
            {
                return View();
            }
        }

        public bool CheckPassword(string Username, string Password)
        {
            bool check = false;
            String CS = ConfigurationManager.ConnectionStrings["DataModel"].ConnectionString;
            SqlConnection connection = new SqlConnection(CS);
            string cmd = "Select count(*) from [User] where [Username]='" + Username + "'and [Password]='" + Password +"'";
            connection.Open();
            SqlCommand command = new SqlCommand(cmd, connection);
            check = Convert.ToBoolean(command.ExecuteScalar());
            connection.Close();
            return check;
        }
        public bool CheckUsername(string Username)
        {
            bool check = false;
            String CS = ConfigurationManager.ConnectionStrings["DataModel"].ConnectionString;
            SqlConnection connection = new SqlConnection(CS);
            string cmd = "Select count(*) from [User] where [Username]='" + Username + "'";
            connection.Open();
            SqlCommand command = new SqlCommand(cmd, connection);
            check = Convert.ToBoolean(command.ExecuteScalar());
            connection.Close();
            return check;
        }
    }
}
