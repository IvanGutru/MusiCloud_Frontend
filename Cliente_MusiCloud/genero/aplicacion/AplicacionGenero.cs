using Cliente_MusiCloud.genero.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cliente_MusiCloud.genero.aplicacion
{
    class AplicacionGenero
    {
        public static async Task<List<Genero>> ObtenerGenerosAsync()
        {
            string path = "Generos";
            List<Genero> listaGeneros;
            using (HttpResponseMessage respuesta = await ConexionApi.ApiCliente.GetAsync(path))
            {
                if (respuesta.IsSuccessStatusCode)
                {
                    listaGeneros = await respuesta.Content.ReadAsAsync<List<Genero>>();
                    return listaGeneros;
                }
                else
                {
                    dynamic error = await respuesta.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new Exception(mensaje);
                }
            }
        }
        public static async Task<List<Genero>> ObtenerIdGeneroAsync(string nombre)
        {
            string path = "Generos/ObtenerId/"+nombre;
            using (HttpResponseMessage respuesta = await ConexionApi.ApiCliente.GetAsync(path))
            {
                if (respuesta.IsSuccessStatusCode)
                {
                    List<Genero> idGenero = await respuesta.Content.ReadAsAsync<List<Genero>>();
                    return idGenero;
                }
                else
                {
                    dynamic error = await respuesta.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new Exception(mensaje);
                }
            }
        }
    }
}
