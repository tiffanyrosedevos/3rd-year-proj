using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BucketList.Models
{
    public class Challenge
    {
        public int challengeID { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int difficulty { get; set; }
        public int points { get; set; }
        public bool needPhoto { get; set; }
        public bool canBeGroup { get; set; }
    }
}