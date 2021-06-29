using System.ComponentModel.DataAnnotations;

namespace QuizAndAnswer.Models {
    public class Question {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter the description for question")]
        public string QuestionDesc { get; set; }
        [Required]
        public bool IsCorrect { get; set; }
        [Required(ErrorMessage = "Please specify amount of points for answering question correctly")]
        public int Points { get; set; }
    }
}
