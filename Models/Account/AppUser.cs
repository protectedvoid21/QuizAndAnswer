using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAndAnswer.Models {
    public class AppUser : IdentityUser<int> {
        [Required]
        public override string UserName { get; set; }
        [Required]
        public override string PasswordHash { get; set; }
        [Required]
        public override string Email { get; set; }
    }
}
