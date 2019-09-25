using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BucketList.Models
{
    public class Friends
    {
        public int userID { get; set; }
        public List<UserFriend> nonFriends { get; set; }
        public List<UserFriend> currentFriends { get; set; }
    }
}