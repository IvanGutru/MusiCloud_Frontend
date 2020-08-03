using Cliente_MusiCloud.cancion.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cliente_MusiCloud.cancion.aplicacion
{
    class AplicacionCancion
    {
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
    }
}
