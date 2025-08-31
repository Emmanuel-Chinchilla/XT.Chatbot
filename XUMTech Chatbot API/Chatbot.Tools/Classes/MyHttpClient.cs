using Chatbot.Tools.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatbot.Tools.Classes
{
    public class MyHttpClient : IMyHttpClient
    {
        private readonly HttpClient _httpClient;

        public MyHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> Send(string json, string apiUrl)
        {

            // Usa _httpClient para enviar la solicitud POST al apiUrl
            // Crear el contenido de la solicitud
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Enviar la solicitud POST al servidor
            HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);

            // Manejar la respuesta del servidor
            if (response.IsSuccessStatusCode)
            {
                // La solicitud fue exitosa
                Console.WriteLine("Solicitud POST exitosa");
            }
            else
            {
                // La solicitud falló
                Console.WriteLine($"Error al enviar la solicitud POST. Código de estado: {response.StatusCode}");
            }

            return response;
        }
        public async Task Send(string apiUrl)
        {
            // Enviar la solicitud POST al servidor
            HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, null);

            // Manejar la respuesta del servidor
            if (response.IsSuccessStatusCode)
            {
                // La solicitud fue exitosa
                Console.WriteLine("Solicitud POST exitosa");
            }
            else
            {
                // La solicitud falló
                Console.WriteLine($"Error al enviar la solicitud POST. Código de estado: {response.StatusCode}");
            }
        }

        public async Task<HttpResponseMessage> Get(string apiUrl)
        {
            // Enviar la solicitud GET al servidor
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            // Manejar la respuesta del servidor
            if (response.IsSuccessStatusCode)
            {
                // La solicitud fue exitosa
                Console.WriteLine("Solicitud GET exitosa");
            }
            else
            {
                // La solicitud falló
                Console.WriteLine($"Error al enviar la solicitud GET. Código de estado: {response.StatusCode}");
            }

            return response;
        }
    }
}
