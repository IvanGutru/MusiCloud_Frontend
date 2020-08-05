using Cliente_MusiCloud.playlist.dominio;
using Cliente_MusiCloud.utilidades;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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
        public static async Task<List<Playlist>> ObtenerPlaylistSistema()
        {
            List<Playlist> listaPlaylist;
            string path = "Playlist/Sistema/1";
            using (HttpResponseMessage respuesta = await ConexionApi.ApiCliente.GetAsync(path))
            {
                if (respuesta.IsSuccessStatusCode)
                {
                    listaPlaylist = await respuesta.Content.ReadAsAsync<List<Playlist>>();
                    return listaPlaylist;
                }
                else
                {
                    dynamic error = await respuesta.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new Exception(mensaje);
                }
            }
        }
        public static async Task<BitmapImage> ObtenerImagenPlaylist(string nombreImagen)
        {
            string path = "Playlist/imagen/"+nombreImagen;
            using (HttpResponseMessage respuesta = await ConexionApi.ApiCliente.GetAsync(path))
            {
                if (respuesta.IsSuccessStatusCode)
                {
                    String imagenBase64 = await respuesta.Content.ReadAsStringAsync();
                    BitmapImage imagen = CodificacionImagenes.DecodificarBase64(imagenBase64);
                    return imagen;
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
