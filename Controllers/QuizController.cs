using Microsoft.AspNetCore.Authorization;
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

        public QuizController(QuestionDbContext questionContext, UserManager<AppUser> userManager) {
            this.questionContext = questionContext;
            this.userManager = userManager;
        }

        [Authorize]
        public ViewResult QuizList() {
            List<Question> questionList = questionContext.Quiz.ToList();
            return View(questionList);
        }

        [Authorize]
        public ViewResult MainQuizDb() {
            return View();
        }

        [HttpGet, Authorize]
        public ViewResult AddQuestion() {
            return View();
        }

        [HttpPost]
        public ViewResult AddQuestion(Question question) {
            if(!ModelState.IsValid) {
                return View();
            }

            question.Id = questionContext.Quiz.ToArray().Last().Id + 1;
            questionContext.Add(question);
            questionContext.SaveChanges();
            return View("QuestionAdded", question);
        }

        [HttpGet, Authorize]
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

        [HttpGet, Authorize]
        public IActionResult Delete(int id) {
            questionContext.Quiz.Remove(questionContext.GetQuestionById(id - 1));
            questionContext.SaveChanges();
            return RedirectToAction("QuizList");
        }

        public ViewResult PrepareTest() {
            return View();
        }

        [HttpGet, Authorize]
        public ViewResult Test(int questionCount) {
            if(questionCount <= 0) {
                ModelState.AddModelError("", "The entered value is below or equal 0");
                return View("PrepareTest");
            }
            if(questionCount > questionContext.Quiz.Count()) {
                ModelState.AddModelError("", "The entered value is bigger than the count of questions in database");
                return View("PrepareTest");
            }

            List<Question> dbList = questionContext.Quiz.ToList();
            List<Question> examList = new List<Question>();

            while(questionCount != 0) {
                Random rng = new Random();
                Question selectedQuestion = dbList[rng.Next(0, dbList.Count)];

                if(!examList.Contains(selectedQuestion)) {
                    examList.Add(selectedQuestion);
                    questionCount--;
                }
            }

            questionList = examList;
            ViewBag.QuizList = examList;
            bool[] answerList = new bool[examList.Count];
            return View(answerList);
        }

        [HttpPost]
        public async Task<IActionResult> Test(bool[] answerList) {
            if(!ModelState.IsValid) {
                return RedirectToAction("Test");
            }

            int points = 0;
            int maxPoints = 0;

            for(int i = 0; i < questionList.Count; i++) {
                maxPoints += questionList[i].Points;
                if(answerList[i] == questionList[i].IsCorrect) {
                    points += questionList[i].Points;
                }
            }
            int dataId = questionContext.UserData.Any() ? questionContext.UserData.ToArray().Last().Id + 1 : 1;
            int userId = int.Parse(userManager.GetUserId(HttpContext.User));
            await questionContext.UserData.AddAsync(new UserQuestionData {
                Id = dataId,
                UserId = userId,
                SubmitDate = DateTime.Now,
                CorrectPoints = points,
                MaxPoints = maxPoints,
                QuestionCount = questionList.Count(),
            });
            await questionContext.SaveChangesAsync();

            ViewData["Points"] = points;
            ViewData["MaxPoints"] = maxPoints;
            ViewData["QuestionList"] = questionList;
            return View("AnswerList", answerList);
        }

        [Authorize]
        public ViewResult UserQuizStats() {
            int userId = int.Parse(userManager.GetUserId(HttpContext.User));
            IEnumerable<UserQuestionData> questions = questionContext.UserData.Where(p => p.UserId == userId);
            return View(questions);
        }
    }
}
