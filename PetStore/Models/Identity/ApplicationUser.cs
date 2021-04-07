using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PetStore.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime Birthday { get; set; }

        [NotMapped]
        public bool IsVIP { get; set; }

        public ApplicationUser()
        {

        }

        public ApplicationUser(string userName) : base(userName)
        {
        }
    }
}
