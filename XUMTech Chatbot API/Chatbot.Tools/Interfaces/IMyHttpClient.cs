using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Tools.Interfaces
{
    public interface IMyHttpClient
    {
        //Send post
        Task<HttpResponseMessage> Send(string json, string apiUrl);

        /// <summary>
        /// Send post con el content Null
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <returns></returns>
        Task Send(string apiUrl);

        Task<HttpResponseMessage> Get(string apiUrl);
    }
}
