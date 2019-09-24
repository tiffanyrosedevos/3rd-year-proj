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
                command = new SqlCommand("Select a.description from Achievement a INNER JOIN UserAchievement u ON a.achievementID = u.achievementID WHERE u.userID = @friendID", conn);
                param = new SqlParameter();
                param.ParameterName = "@friendID";
                param.Value = friendID;
                command.Parameters.Add(param);
                reader = command.ExecuteReader();
                friend.achievements = new List<string>();
                while (reader.Read())
                {
                    String achievement = reader.GetString(0);
                    friend.achievements.Add(achievement);
                }
                conn.Close();
                reader.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            return View(friend);
        }
       
        public ActionResult RemoveFriend(int userID, int friendID)
        {
            try
            {
                string CS = ConfigurationManager.ConnectionStrings["DatabaseEntities1"].ConnectionString;
                SqlConnection conn = new SqlConnection(CS);
                conn.Open();
                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@friendID";
                param1.Value = friendID;
                SqlParameter param2 = new SqlParameter();
                param2.ParameterName = "@userID";
                param2.Value = userID;
                SqlCommand command = new SqlCommand("DELETE FROM Friendship WHERE (userID1 = @userID AND userID2 = @friendID) OR (userID1 = @friendID AND userID2 = @userID)", conn);
                command.Parameters.Add(param1);
                command.Parameters.Add(param2);

                command.ExecuteNonQuery();

                conn.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}