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

            return View(challenge);
        }

        public ActionResult RateChallenge()
        {
            return View();
        }
    }
}