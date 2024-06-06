﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Exoticamp.UI.Models.Users
{
    public class UsersVM
    {
        public string UsersId { get; set; }
        public string Name { get; set; }


        [Required]

        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

 

         

        [Required]
        public string Role { get; set; }
        public bool IsLocked { get; set; }
        public int LoginAttemptCount { get; set; }
    }
}
