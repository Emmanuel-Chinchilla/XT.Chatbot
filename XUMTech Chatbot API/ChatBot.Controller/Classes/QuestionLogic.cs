using Chatbot.Model.Classes;
using ChatBot.Controller.Interfaces;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using Chatbot.Tools.Interfaces;

namespace ChatBot.Controller.Classes
{
    public class QuestionLogic: IQuestionLogic
    {
        private readonly string fileRoute;
        private readonly string apiSystemKey;
        private readonly IMyHttpClient httpClient;
        private readonly string storageAPI;
        private readonly Dictionary<string, string> responses;

        public QuestionLogic(IConfiguration configuration, IMyHttpClient httpClient) {
            fileRoute = configuration["JSONPath"];
            apiSystemKey = configuration["APISystemKey"];
            storageAPI = configuration["XT.Storage"];
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Ejecuta el request al API de almacenamiento y realiza la logica para encontrar una respuesta a la pregunta
        /// </summary>
        /// <param name="question"></param>
        /// <returns></returns>
        async public Task<dynamic> GetAnswer(string question) {
            //Crar el objeto de respuesta
            var answer = new Answer
            {
                user = "Chatbot",
                createdAt = DateTime.Now
            };

            try
            {
                //Construye el payload con la apiSystemKey
                string payload = JsonSerializer.Serialize(keyBody());

                //Ejecutar request a XT.Storage
                HttpResponseMessage responseAPI = await Task.Run(() => httpClient.Send(payload, storageAPI)).ConfigureAwait(true);

                if (responseAPI.IsSuccessStatusCode)
                {
                    //Leer el contenido del GET
                    string jsonString = await responseAPI.Content.ReadAsStringAsync();

                    //Transformar la respuesta a un diccionario de datos
                    var dictionary = TransformData(jsonString);

                    //Buscar la primer coincidencia que se encuentre en el diccionario normalizando el string de la respuesta
                    var key = dictionary.Keys.FirstOrDefault(k => Normalize(k).IndexOf(question, StringComparison.OrdinalIgnoreCase) >= 0);

                    if (key != null)
                    {
                        var response = dictionary[key];

                        response = replaceDinamycMarkers(response);

                        answer.text = response;

                        return answer;
                    }

                    answer.text = "No entendí tu pregunta. Puedes preguntarme cosas como: " + string.Join(", ", dictionary.Keys);

                    return answer;
                }
                else
                {
                    answer.text = $"Ha ocurrido un problema al consultar el servidor de almacenamiento. Codigo de error: {responseAPI.StatusCode}";
                    return answer;
                }
            }
            catch (Exception ex)
            {
                {
                    answer.text = $"Ocurrió un error inesperado al procesar tu pregunta. Motivo: {ex.Message}";
                    return answer;
                }
            }
            
        }

        private Key keyBody() {
            return new Key
            {
                APISystem = apiSystemKey
            };

        }

        /// <summary>
        /// Reemplazar marcadores como la fecha u hora
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private string replaceDinamycMarkers(string response) {

            // Reemplazar marcadores dinámicos si existen
            response = response.Replace("{hora}", DateTime.Now.ToShortTimeString());
            response = response.Replace("{fecha}", DateTime.Now.ToString("dddd, dd MMMM yyyy"));

            return response;
        }

        /// <summary>
        /// Desearializar el JSON de mi respuesta a un diccionario de datos
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private Dictionary<string, string> TransformData(string json) {
            
            Dictionary<string, string> responses = new Dictionary<string, string>();
            try
            {
                //Remover saltos de linea
                string cleaned = json.Replace("\r", "").Replace("\n", "");

                responses = JsonSerializer.Deserialize<Dictionary<string, string>>(cleaned)
                                ?? new Dictionary<string, string>();
                
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.Message);
            }
            
            return responses;
        }

        /// <summary>
        /// Esta función elimina los acentos y otros signos de un texto.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string Normalize(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
