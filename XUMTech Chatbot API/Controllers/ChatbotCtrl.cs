
using Chatbot.Model.Classes;
using ChatBot.Controller.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace XUMTech_Chatbot_API.Controllers
{
    public class ChatbotCtrl : Controller
    {
        private readonly IQuestionLogic? questionLogic;

        public ChatbotCtrl(IQuestionLogic questionLogic)
        {
            this.questionLogic = questionLogic;
        }

        [HttpPost("Ask")]
        async public Task<IActionResult> Ask([FromBody] Question question)
        {
            if (string.IsNullOrWhiteSpace(question.text))
                return BadRequest("La pregunta no puede estar vacía.");

            var response = await questionLogic.GetAnswer(question.text);
            return Ok(response);
        }

    }
}
