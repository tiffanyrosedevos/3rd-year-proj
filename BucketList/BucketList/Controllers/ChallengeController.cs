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
    public class ChallengeController : Controller
    {
        // GET: Challenge
        public ActionResult SuggestChallenge()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SuggestChallenge(Challenge challenge)
        {
            try
            {
                string CS = ConfigurationManager.ConnectionStrings["DatabaseEntities1"].ConnectionString;
                SqlConnection conn = new SqlConnection(CS);
                conn.Open();
                SqlParameter p1 = new SqlParameter();
                p1.ParameterName = "@title";
                p1.Value = challenge.title;
                SqlParameter p2 = new SqlParameter();
                p2.ParameterName = "@description";
                p2.Value = challenge.description;
                SqlParameter p3 = new SqlParameter();
                p3.ParameterName = "@difficulty";
                p3.Value = challenge.difficulty;
                SqlParameter p4 = new SqlParameter();
                p4.ParameterName = "@points";
                p4.Value = challenge.points;
                SqlParameter p5 = new SqlParameter();
                p5.ParameterName = "@needPhoto";
                p5.Value = challenge.needPhoto;
                SqlParameter p6 = new SqlParameter();
                p6.ParameterName = "@canBeGroup";
                p6.Value = challenge.canBeGroup;
                SqlCommand command = new SqlCommand("INSERT INTO Challenge (title, description, difficulty, points, needPhoto, canBeGroup) VALUES (@title, @description, @difficulty, @points, @needPhoto, @canBeGroup)", conn);
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                command.Parameters.Add(p3);
                command.Parameters.Add(p4);
                command.Parameters.Add(p5);
                command.Parameters.Add(p6);              
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult RateChallenge(int userID, int challengeID)
        {
            Rating challengeRating = new Rating();
            challengeRating.userID = userID;
            challengeRating.challengeID = challengeID;
            return View(challengeRating);
        }

        [HttpPost]
        public ActionResult RateChallenge(Rating challengeRating)
        {         
            try
            {
                string CS = ConfigurationManager.ConnectionStrings["DatabaseEntities1"].ConnectionString;
                SqlConnection conn = new SqlConnection(CS);
                conn.Open();
                SqlParameter p1 = new SqlParameter();
                p1.ParameterName = "@challengeID";
                p1.Value = challengeRating.challengeID;
                SqlParameter p2 = new SqlParameter();
                p2.ParameterName = "@userID";
                p2.Value = challengeRating.userID;
                SqlParameter p3 = new SqlParameter();
                p3.ParameterName = "@rating";
                p3.Value = challengeRating.rating;
                SqlParameter p4 = new SqlParameter();
                p4.ParameterName = "@review";
                p4.Value = challengeRating.review;
                SqlCommand command = new SqlCommand("INSERT INTO Rating (challengeID, userID, rating, review) VALUES (@challengeID, @userID, @rating, @review)", conn);
                command.Parameters.Add(p1);
                command.Parameters.Add(p2);
                command.Parameters.Add(p3);
                command.Parameters.Add(p4);
                command.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Congratulations(int userID, int challengeID)
        {
            Challenge challenge = new Challenge();
            try
            {
                string CS = ConfigurationManager.ConnectionStrings["DatabaseEntities1"].ConnectionString;
                SqlConnection conn = new SqlConnection(CS);
                conn.Open();
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@challengeID";
                param.Value = userID;
                SqlCommand command = new SqlCommand("SELECT title, points FROM [dbo].[Challenge] WHERE challengeID = @challengeID", conn);
                command.Parameters.Add(param);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    challenge.title = reader.GetString(0);
                    challenge.points = reader.GetInt32(1);
                }
                conn.Close();
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return View(challenge);
        }

        public ActionResult ViewRatings(int challengeID)
        {
            Ratings ratings = new Ratings();
            try
            {
                string CS = ConfigurationManager.ConnectionStrings["DatabaseEntities1"].ConnectionString;
                SqlConnection conn = new SqlConnection(CS);
                conn.Open();
                SqlParameter p = new SqlParameter();
                p.ParameterName = "@challengeID";
                p.Value = challengeID;
                SqlCommand command = new SqlCommand("SELECT u.firstName, u.surname, r.rating, r.review FROM [dbo].[User] AS u INNER JOIN [dbo].[Rating] AS r ON u.userID = r.userID WHERE challengeId = @challengeID", conn);
                command.Parameters.Add(p);
                SqlDataReader reader = command.ExecuteReader();
                ratings.ratings = new List<Rating>();
                while (reader.Read())
                {
                    Rating rating = new Rating();
                    rating.firstName = reader.GetString(0);
                    rating.surname = reader.GetString(1);
                    rating.rating = reader.GetInt32(2);
                    rating.review = reader.GetString(3);                   
                    ratings.ratings.Add(rating);
                }
                conn.Close();
                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return View(ratings);
        }

        public ActionResult CreateChallenge()
        {
            return View();
        }
        public ActionResult MaintainChallenge()
        {
            return View();
        }
     
    }
}