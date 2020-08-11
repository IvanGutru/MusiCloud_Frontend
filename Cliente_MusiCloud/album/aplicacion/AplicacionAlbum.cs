using Cliente_MusiCloud.album.dominio;
using Cliente_MusiCloud.utilidades;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Cliente_MusiCloud.album.aplicacion
{
    public class AplicacionAlbum
    {
        public static async Task<List<Album>> ObtenerAlbumesArtistaPorId(String idArtista)
        {
            String path = "Album/" + idArtista;
            List<Album> listaAlbumes;
            using (HttpResponseMessage response = await ConexionApi.ApiCliente.GetAsync(path))
            {
                if (response.IsSuccessStatusCode)
                {
                    listaAlbumes = await response.Content.ReadAsAsync<List<Album>>();
                    return listaAlbumes;
                }
                else
                {
                    dynamic error = await response.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new FormatException(mensaje);
                }
            }
        }
        public static async Task<List<Album>> ObtenerAlbumPorNombre(String nombreAlbum)
        {
            string path = "Album/Nombre/" + nombreAlbum;
            List<Album> listaAlbumes;
            using (HttpResponseMessage response = await ConexionApi.ApiCliente.GetAsync(path))
            {
                if (response.IsSuccessStatusCode)
                {
                    listaAlbumes = await response.Content.ReadAsAsync<List<Album>>();
                    return listaAlbumes;
                }
                else
                {
                    dynamic error = await response.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new FormatException(mensaje);
                }
            }
        }
        public static async Task<List<Album>> ObtenerAlbumPorId(String idAlbum)
        {
            string path = "Album/Id/" + idAlbum;
            List<Album> listaAlbumes;
            using (HttpResponseMessage response = await ConexionApi.ApiCliente.GetAsync(path))
            {
                if (response.IsSuccessStatusCode)
                {
                    listaAlbumes = await response.Content.ReadAsAsync<List<Album>>();
                    return listaAlbumes;
                }
                else
                {
                    dynamic error = await response.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new FormatException(mensaje);
                }
            }
        }
        public static async Task<Album> GuardarAlbum(Album album)
        {
            Album albumRegistrado;
            string path = "Album";
            using (HttpResponseMessage respuesta = await ConexionApi.ApiCliente.PostAsJsonAsync(path,album))
            {
                if (respuesta.IsSuccessStatusCode)
                {
                    albumRegistrado = await respuesta.Content.ReadAsAsync<Album>();
                    return albumRegistrado;
                }
                else
                {
                    dynamic error = await respuesta.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new FormatException(mensaje);
                }
            }
        }
        public static async Task<List<Album>> ObtenerAlbumHome()
        {
            string path = "AlbumHome";
            List<Album> listaAlbumes;
            using (HttpResponseMessage response = await ConexionApi.ApiCliente.GetAsync(path))
            {
                if (response.IsSuccessStatusCode)
                {
                    listaAlbumes = await response.Content.ReadAsAsync<List<Album>>();
                    return listaAlbumes;
                }
                else
                {
                    dynamic error = await response.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new FormatException(mensaje);
                }

            }
        }
        public static async Task<BitmapImage> ObtenerImagenAlbum(string nombreImagen)
        {
            string path = "Album/imagen/" + nombreImagen;
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
                    throw new FormatException(mensaje);
                }
            }
        }
    }
}
