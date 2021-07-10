using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAndAnswer.Models {
    public class UserQuestionData {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime SubmitDate { get; set; }
        public int CorrectPoints { get; set; }
        public int MaxPoints { get; set; }

        public int CorrectPercent() {
            return (int)((float)CorrectPoints / MaxPoints * 100);
        }
    }
}
