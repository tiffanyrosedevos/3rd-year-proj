using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BucketList.Models
{
    public class Rating
    {
        public int ratingID { get; set; }
        public int challengeID { get; set; }
        public int userID { get; set; }
        public int rating { get; set; }
        public string review { get; set; }

        public string firstName { get; set; } // Can I put this here? I'm using it in the ViewRatings view
        public string surname { get; set; }
    }
}