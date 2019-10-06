using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BucketList.Models
{
    public class Invite
    {
        public int userID { get; set; }
        public int friendID { get; set; }
        public string username { get; set; }
        public string firstName { get; set; }
        public string surname { get; set; }
        public int points { get; set; }
        public string inviteType { get; set; }
        public int inviteID { get; set; }
        public Challenge challenge { get; set; }
    }
}