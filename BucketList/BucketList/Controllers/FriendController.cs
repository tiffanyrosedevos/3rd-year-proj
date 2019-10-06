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
            catch (Exception e)
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
            catch (Exception e)
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

                //get all friends
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
                //get all users
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
                //remove friends
                nonFriends = nonFriends.Where(f => !friends.currentFriends.Any(f2 => f2.friendID == f.friendID)).ToList();
                //remove user himself
                friends.nonFriends = nonFriends.Where(f => f.friendID != userID).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return View(friends);
        }

        public void SendFriendRequest(int userID, int friendID)
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
                SqlCommand command = new SqlCommand("INSERT INTO FriendInvite (fromUserID, toUserID) VALUES (@userID,@friendID)", conn);
                command.Parameters.Add(param1);
                command.Parameters.Add(param2);
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void AcceptFriendRequest(int userID, int friendID)
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
                SqlCommand command = new SqlCommand("INSERT INTO Friendship (userID1, userID2) VALUES (@userID,@friendID)", conn);
                command.Parameters.Add(param1);
                command.Parameters.Add(param2);
                command.ExecuteNonQuery();

                param1 = new SqlParameter();
                param1.ParameterName = "@friendID";
                param1.Value = friendID;
                param2 = new SqlParameter();
                param2.ParameterName = "@userID";
                param2.Value = userID;
                command = new SqlCommand("DELETE FROM FriendInvite WHERE fromUserID = @friendID AND toUserID = @userID", conn);
                command.Parameters.Add(param1);
                command.Parameters.Add(param2);
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void RejectFriendRequest(int userID, int friendID)
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
                SqlCommand command = new SqlCommand("DELETE FROM FriendInvite WHERE fromUserID = @friendID AND toUserID = @userID", conn);
                command.Parameters.Add(param1);
                command.Parameters.Add(param2);
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void GetFriendRequests(int userID)
        {
            Invites invites = new Invites();
            invites.userID = userID;
            try
            {
                string CS = ConfigurationManager.ConnectionStrings["DatabaseEntities1"].ConnectionString;
                SqlConnection conn = new SqlConnection(CS);
                conn.Open();
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@userID";
                param.Value = userID;
                SqlCommand command = new SqlCommand("select u.userID, u.username, u.firstName, u.surname, u.points from [dbo].[User] u INNER JOIN FriendInvite f ON u.userID = f.fromUserID where f.toUserID = @userID", conn);
                command.Parameters.Add(param);
                SqlDataReader reader = command.ExecuteReader();
                invites.invites = new List<Invite>();
                while (reader.Read())
                {
                    Invite invite = new Invite();
                    invite.friendID = reader.GetInt32(0);
                    invite.username = reader.GetString(1);
                    invite.firstName = reader.GetString(2);
                    invite.surname = reader.GetString(3);
                    invite.points = reader.GetInt32(4);
                    invite.inviteType = "friend";
                    invites.invites.Add(invite);
                }
                conn.Close();
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void GetChallengeInvites(int userID)
        {
            Invites invites = new Invites();
            invites.userID = userID;
            try
            {
                string CS = ConfigurationManager.ConnectionStrings["DatabaseEntities1"].ConnectionString;
                SqlConnection conn = new SqlConnection(CS);
                conn.Open();
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@userID";
                param.Value = userID;
                SqlCommand command = new SqlCommand("select u.userID, u.username, u.firstName, u.surname, u.points, f.challengeInviteID from [dbo].[User] u INNER JOIN GroupChallengeInvite f ON u.userID = f.fromUserID where f.toUserID = @userID AND status = 'pending'", conn);
                command.Parameters.Add(param);
                SqlDataReader reader = command.ExecuteReader();
                invites.invites = new List<Invite>();
                while (reader.Read())
                {
                    Invite invite = new Invite();
                    invite.friendID = reader.GetInt32(0);
                    invite.username = reader.GetString(1);
                    invite.firstName = reader.GetString(2);
                    invite.surname = reader.GetString(3);
                    invite.points = reader.GetInt32(4);
                    invite.inviteID = reader.GetInt32(5);
                    invite.inviteType = "challenge";
                    invites.invites.Add(invite);
                }
                conn.Close();
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public ActionResult ViewChallengeInvite(int userID, int challengeInviteID)
        {
            Invite invite = new Invite();
            try
            {
                string CS = ConfigurationManager.ConnectionStrings["DatabaseEntities1"].ConnectionString;
                SqlConnection conn = new SqlConnection(CS);
                conn.Open();
                //Get user information for challenge invite
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@inviteID";
                param.Value = challengeInviteID;
                SqlCommand command = new SqlCommand("select u.userID, u.username, u.firstName, u.surname, u.points, i.challengeInviteID, i.challengeID from [dbo].[User] u INNER JOIN GroupChallengeInvite i ON u.userID = i.fromUserID where i.challengeInviteID = @inviteID", conn);
                command.Parameters.Add(param);
                SqlDataReader reader = command.ExecuteReader();
                int challengeID = 0;
                while (reader.Read())
                {
                    invite.friendID = reader.GetInt32(0);
                    invite.username = reader.GetString(1);
                    invite.firstName = reader.GetString(2);
                    invite.surname = reader.GetString(3);
                    invite.points = reader.GetInt32(4);
                    invite.inviteID = reader.GetInt32(5);
                    challengeID = reader.GetInt32(6);
                }
                //Get challenge information for challenge invite
                param = new SqlParameter();
                param.ParameterName = "@challengeID";
                param.Value = challengeID;
                command = new SqlCommand("select challengeID, title, description, difficulty, points, needPhoto,canBeGroup FROM Challenge WHERE challengeID = @challengeID", conn);
                command.Parameters.Add(param);
                reader = command.ExecuteReader();
                invite.challenge = new Challenge();
                while (reader.Read())
                {
                    invite.challenge.challengeID = reader.GetInt32(0);
                    invite.challenge.title = reader.GetString(1);
                    invite.challenge.description = reader.GetString(2);
                    invite.challenge.difficulty = reader.GetInt32(3);
                    invite.challenge.points = reader.GetInt32(4);
                    invite.challenge.needPhoto = reader.GetBoolean(5);
                    invite.challenge.canBeGroup = reader.GetBoolean(6);
                }
                conn.Close();
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return View(invite);
        }

        public void AcceptChallengeInvite(int userID, int challengeInviteID)
        {
            try
            {
                string CS = ConfigurationManager.ConnectionStrings["DatabaseEntities1"].ConnectionString;
                SqlConnection conn = new SqlConnection(CS);
                conn.Open();
                //Change invite status
                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@inviteID";
                param1.Value = challengeInviteID;
                SqlCommand command = new SqlCommand("UPDATE GroupChallengeInvite SET status = 'accepted' WHERE challengeInviteID = @inviteID", conn);
                command.Parameters.Add(param1);
                command.ExecuteNonQuery();
                //Get challenge id
                param1 = new SqlParameter();
                param1.ParameterName = "@inviteID";
                param1.Value = challengeInviteID;
                command = new SqlCommand("SELECT challengeID from GroupChallengeInvite WHERE challengeInviteID = @inviteID", conn);
                command.Parameters.Add(param1);
                SqlDataReader reader = command.ExecuteReader();
                int challengeID = 0;
                while (reader.Read())
                {
                    challengeID = reader.GetInt32(0);
                }
                //add challenge into user challenge for this user
                param1 = new SqlParameter();
                param1.ParameterName = "@challengeID";
                param1.Value = challengeID;
                SqlParameter param2 = new SqlParameter();
                param2.ParameterName = "@userID";
                param2.Value = userID;
                command = new SqlCommand("INSERT INTO UserChallenge (userID, challengeID, status) VALUES (@userID, @challengeID, 'uncompleted')", conn);
                command.Parameters.Add(param1);
                command.Parameters.Add(param2);
                command.ExecuteNonQuery();
                conn.Close();
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void RejectChallengeInvite(int userID, int challengeInviteID)
        {
            try
            {
                string CS = ConfigurationManager.ConnectionStrings["DatabaseEntities1"].ConnectionString;
                SqlConnection conn = new SqlConnection(CS);
                conn.Open();
                //Delete group challenge invite
                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@inviteID";
                param1.Value = challengeInviteID;
                SqlCommand command = new SqlCommand("DELETE FROM GroupChallengeInvite WHERE challengeInviteID = @inviteID", conn);
                command.Parameters.Add(param1);
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void VerifyChallengeCompletion(int userID, int challengeID)
        {
            try
            {
                string CS = ConfigurationManager.ConnectionStrings["DatabaseEntities1"].ConnectionString;
                SqlConnection conn = new SqlConnection(CS);
                conn.Open();
                //Change invite status
                SqlParameter param1 = new SqlParameter();
                param1.ParameterName = "@challengeID";
                param1.Value = challengeID;
                SqlParameter param2 = new SqlParameter();
                param2.ParameterName = "@userID";
                param2.Value = userID;
                SqlCommand command = new SqlCommand("UPDATE UserChallenge SET status = 'verified' WHERE challengeID = @challengeID AND userID = @userID", conn);
                command.Parameters.Add(param1);
                command.Parameters.Add(param2);
                command.ExecuteNonQuery();

                //Find other users if its a group challenge
                List<int> users = new List<int>();
                param1 = new SqlParameter();
                param1.ParameterName = "@challengeID";
                param1.Value = challengeID;
                param2 = new SqlParameter();
                param2.ParameterName = "@userID";
                param2.Value = userID;
                command = new SqlCommand("SELECT toUserID FROM GroupChallengeInvite WHERE challengeID = @challengeID AND fromUserID = @userID AND status = 'accepted'", conn);
                command.Parameters.Add(param1);
                command.Parameters.Add(param2);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int user = reader.GetInt32(0);
                    users.Add(user);
                }
                reader.Close();

                //update status for each user challenge
                foreach(int user in users)
                {
                    param1 = new SqlParameter();
                    param1.ParameterName = "@challengeID";
                    param1.Value = challengeID;
                    param2 = new SqlParameter();
                    param2.ParameterName = "@userID";
                    param2.Value = user;
                    command = new SqlCommand("UPDATE UserChallenge SET status = 'verified' WHERE challengeID = @challengeID AND userID = @userID", conn);
                    command.Parameters.Add(param1);
                    command.Parameters.Add(param2);
                    command.ExecuteNonQuery();
                }
                //delete group challenge
                param1 = new SqlParameter();
                param1.ParameterName = "@challengeID";
                param1.Value = challengeID;
                param2 = new SqlParameter();
                param2.ParameterName = "@userID";
                param2.Value = userID;
                command = new SqlCommand("DELETE FROM GroupChallengeInvite WHERE challengeID = @challengeID AND fromUserID = @userID", conn);
                command.Parameters.Add(param1);
                command.Parameters.Add(param2);
                command.ExecuteNonQuery();
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}