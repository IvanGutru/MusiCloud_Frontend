using Cliente_MusiCloud.playlistCanciones.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cliente_MusiCloud.playlistCanciones.aplicacion
{
    class AplicacionPlaylistCanciones
    {
        public static async Task<bool> AgregarCancionAPlaylist(int idPlaylist, string idCancion)
        {
            string path = "PlaylistCanciones/" +idPlaylist+"/"+idCancion;
            using (HttpResponseMessage respuesta = await ConexionApi.ApiCliente.PostAsync(path, null))
            {
                if (respuesta.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    dynamic error = await respuesta.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new FormatException(mensaje);
                }
            }
        }

        public static async Task<List<PlaylistCanciones>> ObtenerPlaylistCanciones(int idPlaylist)
        {
            string path = "PlaylistCanciones/" + idPlaylist;
            List<PlaylistCanciones> listaPlaylistCanciones;
            using (HttpResponseMessage respuesta = await ConexionApi.ApiCliente.GetAsync(path))
            {
                if (respuesta.IsSuccessStatusCode)
                {
                    listaPlaylistCanciones =await respuesta.Content.ReadAsAsync <List<PlaylistCanciones>>();
                    return listaPlaylistCanciones;
                }
                else
                {
                    dynamic error = await respuesta.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new FormatException(mensaje);
                }
            }
        }
    }
}
