using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BucketList.Models
{
    public class User
    {

        public int userID { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public int password { get; set; }
        [Required]
        public int confirmPassword { get; set; }
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Required]
        public string email { get; set; }
        public int points { get; set; }

        public List<String> achievements { get; set; } // just string of the name
    }
}