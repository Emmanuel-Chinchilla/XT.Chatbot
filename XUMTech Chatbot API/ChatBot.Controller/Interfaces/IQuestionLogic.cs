using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatBot.Controller.Interfaces
{
    public interface IQuestionLogic
    {
        Task<dynamic> GetAnswer(string question);
    }
}
