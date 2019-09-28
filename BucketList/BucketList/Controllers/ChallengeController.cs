using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BucketList.Controllers
{
    public class ChallengeController : Controller
    {
        // GET: Challenge
        public ActionResult SuggestChallenge()
        {
            return View();
        }

        public ActionResult RateChallenge()
        {
            return View();
        }
    }
}