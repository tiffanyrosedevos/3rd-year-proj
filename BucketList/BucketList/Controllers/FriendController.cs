using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BucketList.Models;

namespace BucketList.Controllers
{
    public class FriendController : Controller
    {
        [HttpGet]
        public ActionResult ViewFriendProfile(int userID, int friendID)
        {
            UserFriend friend = new UserFriend();
            friend.userID = userID;
            friend.friendID = friendID;
            try
            {
                string CS = ConfigurationManager.ConnectionStrings["DatabaseEntities1"].ConnectionString;
                SqlConnection conn = new SqlConnection(CS);
                conn.Open();
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@friendID";
                param.Value = friendID;
                SqlCommand command = new SqlCommand("select firstName, surname, points from [dbo].[User] where userID = @friendID", conn);
                //SqlCommand command = new SqlCommand("select * from Challenge", conn);

                command.Parameters.Add(param);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    friend.firstName = reader.GetString(0);
                    friend.surname = reader.GetString(1);
                    friend.points = reader.GetInt32(2);
                }
                conn.Close();
                reader.Close();
            }
            catch(Exception e)
            {
                
            }

            return View(friend);
        }
    }
}