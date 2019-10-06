using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BucketList.Models
{
    public class UserChallenge
    {
        public int userID { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string surname { get; set; }
        public int userPoints { get; set; }
        public int challengeID { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int difficulty { get; set; }
        public int challengePoints { get; set; }
        public bool needPhoto { get; set; }
        public bool canBeGroup { get; set; }
        public string challengeStatus { get; set; }
    }
}