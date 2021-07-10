using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizAndAnswer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizAndAnswer.Controllers {
    public class QuizController : Controller {
        private readonly QuestionDbContext questionContext;
        private readonly UserManager<AppUser> userManager;

        private static List<Question> questionList;
        private static bool isRandomized;

        public QuizController(QuestionDbContext questionContext, UserManager<AppUser> userManager) {
            this.questionContext = questionContext;
            this.userManager = userManager;
        }

        public ViewResult QuizList() {
            List<Question> questionList = questionContext.Quiz.ToList();
            return View(questionList);
        }

        public ViewResult MainQuizDb() {
            return View();
        }

        [HttpGet]
        public ViewResult AddQuestion() {
            return View();
        }

        [HttpPost]
        public ViewResult AddQuestion(Question question) {
            if (!ModelState.IsValid) {
                return View();
            }

            question.Id = questionContext.Quiz.ToArray().Last().Id + 1;
            questionContext.Add(question);
            questionContext.SaveChanges();
            return View("QuestionAdded", question);   
        }

        [HttpGet]
        public ViewResult Edit(int id) {
            return View(questionContext.GetQuestionById(id));
        }

        [HttpPost]
        public ViewResult Edit(Question question) {
            if(!ModelState.IsValid) {
                return View();
            }

            questionContext.Quiz.ToArray()[question.Id].QuestionDesc = question.QuestionDesc;
            questionContext.Quiz.ToArray()[question.Id].IsCorrect = question.IsCorrect;
            questionContext.Quiz.ToArray()[question.Id].Points = question.Points;
            questionContext.SaveChanges();
            return View("AddedCorrectly", question);
        }

        [HttpGet]
        public IActionResult Delete(int id) {
            questionContext.Quiz.Remove(questionContext.GetQuestionById(id - 1));
            questionContext.SaveChanges();
            return RedirectToAction("QuizList");
        }
        
        [HttpGet]
        public ViewResult Test() {
            if (!isRandomized) {
                isRandomized = true;

                List<Question> dbList = questionContext.Quiz.ToList();
                int randomCount = 5;
                List<Question> examList = new List<Question>();


                while (randomCount != 0) {
                    Random rng = new Random();
                    Question selectedQuestion = dbList[rng.Next(0, dbList.Count)];
                    bool isRepeated = false;

                    foreach (var question in examList) {
                        if (question == selectedQuestion) {
                            isRepeated = true;
                            break;
                        }
                    }
                    if (!isRepeated) {
                        examList.Add(selectedQuestion);
                        randomCount--;
                    }
                }
                questionList = examList;
            }

            ViewBag.QuizList = questionList;
            bool[] answerList = new bool[questionList.Count];
            return View(answerList);
        }

        [HttpPost]
        public async Task<IActionResult> Test(bool[] answerList) {
            if(!ModelState.IsValid) {
                return RedirectToAction("Test");
            }
            isRandomized = false;

            int points = 0;
            int maxPoints = 0;
            
            for(int i = 0; i < questionList.Count; i++) {
                maxPoints += questionList[i].Points;
                if(answerList[i] == questionList[i].IsCorrect) {
                    points += questionList[i].Points;
                }
            }
            int dataId;
            if(questionContext.UserData.Any()) {
                dataId = questionContext.UserData.ToArray().Last().Id + 1;
            }
            else {
                dataId = 1;
            }

            int userId = int.Parse(userManager.GetUserId(HttpContext.User));
            await questionContext.UserData.AddAsync(new UserQuestionData {
                Id = dataId,
                UserId = userId,
                SubmitDate = DateTime.Now,
                CorrectPoints = points,
                MaxPoints = maxPoints,
            });
            await questionContext.SaveChangesAsync();

            ViewData["Points"] = points;
            ViewData["MaxPoints"] = maxPoints;
            ViewData["QuestionList"] = questionList;
            return View("AnswerList", answerList);
        }

        public ViewResult UserQuizStats() {
            int userId = int.Parse(userManager.GetUserId(HttpContext.User));
            IEnumerable<UserQuestionData> questions = questionContext.UserData.Where(p => p.UserId == userId);
            return View(questions);
        }
    }
}
