using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ApiTest
{
    public class FactilizaResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        [JsonProperty("data")]
        public PersonaData Data { get; set; }
    }
    public class PersonaData
    {
        [JsonProperty("numero")]
        public string Numero { get; set; }
        [JsonProperty("nombre_completo")]
        public string NombreCompleto { get; set; }
        [JsonProperty("nombres")]
        public string Nombres { get; set; }
        [JsonProperty("apellido_paterno")]
        public string ApellidoPaterno { get; set; }
        [JsonProperty("apellido_materno")]
        public string ApellidoMaterno { get; set; }
    }
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private const string FactilizaToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIzODc5NyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6ImNvbnN1bHRvciJ9.Q0VZazBtc9rEHECee5F31lOommlqF8eM0uiQuh-hH8A";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Probando conexión a la API de Factiliza...");

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string numeroDoc = "75338725";
            string apiUrl = $"https://api.factiliza.com/v1/dni/info/{numeroDoc}";

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", FactilizaToken);

                Console.WriteLine($"Enviando petición a: {apiUrl}");
                HttpResponseMessage response = await client.SendAsync(request);

                Console.WriteLine($"Respuesta recibida. Código de estado: {(int)response.StatusCode} ({response.ReasonPhrase})");

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("¡CONEXIÓN EXITOSA!");
                    Console.WriteLine("Respuesta JSON:");
                    Console.WriteLine(jsonResponse);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("FALLO EN LA COMUNICACIÓN.");
                    Console.WriteLine("Contenido de la respuesta (si lo hay):");
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(errorContent);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("SE PRODUJO UNA EXCEPCIÓN GRAVE:");
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ResetColor();
                Console.WriteLine("\nPrueba finalizada. Presione cualquier tecla para salir.");
                Console.ReadKey();
            }
        }
    }
}
