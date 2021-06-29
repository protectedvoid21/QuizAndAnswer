using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAndAnswer.Models {
    public class QuestionDbContext : DbContext {
        public QuestionDbContext(DbContextOptions<QuestionDbContext> options) : base(options) { }

        public DbSet<Question> Quiz { get; set; }

        public Question GetQuestionById(int id) {
            return Quiz.ToArray()[id];
        }
    }
}
