using System;
using System.Net.Http;

namespace Cliente_MusiCloud
{
    public class ConexionApi
    {
        public static HttpClient ApiCliente { get; set; }

        private ConexionApi() { }

        public static void Initialize()
        {
            ApiCliente = new HttpClient();
            ApiCliente.BaseAddress = new Uri("http://127.0.0.1:5000/");
            ApiCliente.DefaultRequestHeaders.Accept.Clear();
            ApiCliente.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
