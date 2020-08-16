using System;
using System.Net.Http;

namespace Cliente_MusiCloud
{
    public class ConexionApi
    {
        public static HttpClient ApiCliente { get; set; }
        private const String PUERTO = "5000/";
        private const String DIRECCION = "http://localhost";
        private ConexionApi() { }

        public static void Initialize()
        {
            ApiCliente = new HttpClient();
            ApiCliente.BaseAddress = new Uri(DIRECCION+":"+PUERTO);
            ApiCliente.DefaultRequestHeaders.Accept.Clear();
            ApiCliente.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
