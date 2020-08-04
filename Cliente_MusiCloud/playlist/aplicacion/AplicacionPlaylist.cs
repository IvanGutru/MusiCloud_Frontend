using Cliente_MusiCloud.playlist.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cliente_MusiCloud.playlist.aplicacion
{
    class AplicacionPlaylist
    {
        public static async Task<bool> CrearPlaylist(Playlist playlist)
        {
            String path = "Playlist";
            using (HttpResponseMessage respuesta =await ConexionApi.ApiCliente.PostAsJsonAsync(path,playlist))
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
    }
}
