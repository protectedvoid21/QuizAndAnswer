using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAndAnswer.Models {
    public class UserQuestionData {
        public int Id { get; set; }
        public int AnsweredCorrectly { get; set; }
        public int QuestionCount { get; set; }
    }
}
