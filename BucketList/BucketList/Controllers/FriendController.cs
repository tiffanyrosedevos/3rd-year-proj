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

        public ActionResult ViewFriendList(int userID)
        {
            Friends friends = new Friends();
            friends.userID = userID;
            try
            {
                string CS = ConfigurationManager.ConnectionStrings["DatabaseEntities1"].ConnectionString;
                SqlConnection conn = new SqlConnection(CS);
                conn.Open();
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@userID";
                param.Value = userID;
                SqlCommand command = new SqlCommand("select u.userID, u.username, u.firstName, u.surname, u.points from [dbo].[User] u INNER JOIN Friendship f ON u.userID = f.userID1 where f.userID2 = @userID", conn);
                command.Parameters.Add(param);
                SqlDataReader reader = command.ExecuteReader();
                friends.currentFriends = new List<UserFriend>();
                while (reader.Read())
                {
                    UserFriend friend = new UserFriend();
                    friend.friendID = reader.GetInt32(0);
                    friend.username = reader.GetString(1);
                    friend.firstName = reader.GetString(2);
                    friend.surname = reader.GetString(3);
                    friend.points = reader.GetInt32(4);
                    friends.currentFriends.Add(friend);
                }
                param = new SqlParameter();
                param.ParameterName = "@userID";
                param.Value = userID;
                command = new SqlCommand("select u.userID, u.username, u.firstName, u.surname, u.points from [dbo].[User] u INNER JOIN Friendship f ON u.userID = f.userID2 where f.userID1 = @userID", conn);

                command.Parameters.Add(param);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserFriend friend = new UserFriend();
                    friend.friendID = reader.GetInt32(0);
                    friend.username = reader.GetString(1);
                    friend.firstName = reader.GetString(2);
                    friend.surname = reader.GetString(3);
                    friend.points = reader.GetInt32(4);
                    friends.currentFriends.Add(friend);
                }
                command = new SqlCommand("select userID, username, firstName, surname, points from [dbo].[User]", conn);
                List<UserFriend> nonFriends = new List<UserFriend>();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserFriend friend = new UserFriend();
                    friend.friendID = reader.GetInt32(0);
                    friend.username = reader.GetString(1);
                    friend.firstName = reader.GetString(2);
                    friend.surname = reader.GetString(3);
                    friend.points = reader.GetInt32(4);
                    nonFriends.Add(friend);
                }
                conn.Close();
                reader.Close();
                friends.nonFriends = nonFriends.Where(f => !friends.currentFriends.Any(f2 => f2.friendID == f.friendID)).ToList();
                //TODO: remove user himself
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return View(friends);
        }
    }
}