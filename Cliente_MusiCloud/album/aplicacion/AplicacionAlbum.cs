using Cliente_MusiCloud.album.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cliente_MusiCloud.album.aplicacion
{
    class AplicacionAlbum
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
                    throw new Exception(mensaje);
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
                    throw new Exception(mensaje);
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
                    throw new Exception(mensaje);
                }
            }
        }
    }
}
