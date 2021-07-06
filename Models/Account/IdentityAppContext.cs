using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace QuizAndAnswer.Models {
    public class IdentityAppContext : IdentityDbContext<AppUser, AppRole, int> {
        public IdentityAppContext(DbContextOptions<IdentityAppContext> options) : base(options) {

        }
    }
}
