using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BucketList.Models
{
    public class UserFriends
    {
        public int userID { get; set; }
        public List<UserChallenge> friendChallenges { get; set; }
    }
}