using Cliente_MusiCloud.cancion.dominio;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cliente_MusiCloud.cancion.aplicacion
{
    class AplicacionCancion
    {
        public static async Task<bool> CrearCancion(Cancion cancion)
        {
            string path = "Cancion";
            using (HttpResponseMessage respuesta = await ConexionApi.ApiCliente.PostAsJsonAsync(path,cancion))
            {
                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    dynamic error = await respuesta.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new Exception(mensaje);
                }
            }
        }
        public static async Task<List<Cancion>> ObtenerCancionesPorIdAlbumAsync(String idAlbum)
        {
            String path = "Canciones/" + idAlbum;
            List<Cancion> listaCanciones;
            using (HttpResponseMessage response = await ConexionApi.ApiCliente.GetAsync(path))
            {
                if (response.IsSuccessStatusCode)
                {
                    listaCanciones = await response.Content.ReadAsAsync<List<Cancion>>();
                    return listaCanciones;
                }
                else
                {
                    dynamic error = await response.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new Exception(mensaje);

                }
            }
        }

        public static async Task<Cancion> ObtenerCancionPorId(String idCancion)
        {
            String path = "Canciones/Id/" + idCancion;
            Cancion cancion;
            using (HttpResponseMessage response = await ConexionApi.ApiCliente.GetAsync(path))
            {
                if (response.IsSuccessStatusCode)
                {
                    cancion = await response.Content.ReadAsAsync<Cancion>();
                    return cancion;
                }
                else
                {
                    dynamic error = await response.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new Exception(mensaje);

                }
            }
        }
    }
}
