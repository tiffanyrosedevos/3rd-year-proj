using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BucketList.Views.Home
{
    public class FriendController : Controller
    {
        [HttpGet]
        public ActionResult getFriendProfile(int userID, int friendID)
        {


            try
            {
                string CS = ConfigurationManager.ConnectionStrings["DatabaseEntities1"].ConnectionString;
                SqlConnection conn = new SqlConnection(CS);
                conn.Open();
                SqlCommand command = new SqlCommand("Select * from Challenge", conn);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                }
                conn.Close();
                reader.Close();

            }
            catch
            {

            }

            return View();
        }
    }
}