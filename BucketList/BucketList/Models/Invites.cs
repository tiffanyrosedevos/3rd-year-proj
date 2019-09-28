using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BucketList.Models
{
    public class Invites
    {
        public int userID { get; set; }
        public List<Invite> invites { get; set; }
    }
}