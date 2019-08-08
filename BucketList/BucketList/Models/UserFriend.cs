using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BucketList.Models
{
    public class UserFriend
    {
        public int userID { get; set; }
        public int friendID { get; set; }
        public string firstName { get; set; }
        public string surname { get; set; }
        public int points { get; set; }

        public List<String> achievements { get; set; } // just string of the name
    }
}