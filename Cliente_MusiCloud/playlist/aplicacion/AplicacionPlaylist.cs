using Cliente_MusiCloud.playlist.dominio;
using Cliente_MusiCloud.utilidades;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Cliente_MusiCloud.playlist.aplicacion
{
    static class AplicacionPlaylist
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
        public static async Task<List<Playlist>> ObtenerPlaylistDeUsuario(string idCuenta)
        {
            List<Playlist> listaPlaylist;
            string path = "Playlist/Usuario/"+idCuenta;
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

        public static async Task<List<Playlist>> ObtenerPlaylistTipoUsuario(string idCuenta)
        {
            List<Playlist> listaPlaylist;
            string path = "Playlist/TipoUsuario/" + idCuenta;
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
        public static async Task<bool> ValidarCancionEnMeGusta(string idCancion,string idCuenta)
        {
            string path = "Playlist/MeGusta/"+idCancion+"/"+idCuenta;
            string estadoOk = "OK";
            using (HttpResponseMessage respuesta = await ConexionApi.ApiCliente.GetAsync(path))
            {
                if (respuesta.IsSuccessStatusCode)
                {
                    string estado = respuesta.StatusCode.ToString();
                    if (estado == estadoOk)
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    dynamic error = await respuesta.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new Exception(mensaje);
                }
            }
        }
        public static async Task<bool> AgregarMeGusta(string idCancion, string idCuenta)
        {
            string path = "Playlist/MeGusta/" + idCancion + "/" + idCuenta;
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
                    throw new Exception(mensaje);
                }
            }
        }
        public static async Task<bool> EliminarDeMeGusta(string idCancion, string idCuenta)
        {
            string path = "Playlist/MeGusta/" + idCancion + "/" + idCuenta;
            using (HttpResponseMessage respuesta = await ConexionApi.ApiCliente.DeleteAsync(path))
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
