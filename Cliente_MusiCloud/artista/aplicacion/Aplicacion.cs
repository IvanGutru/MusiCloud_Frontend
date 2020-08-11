using Cliente_MusiCloud.artista.Dominio;
using Cliente_MusiCloud.utilidades;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Cliente_MusiCloud.artista.aplicacion
{
    class Aplicacion
    {
        public static async Task<Artista> RegistrarArtista(Artista artista)
        {
            string path = "Artista";
            Artista artistaRecuperado = null;
            using (HttpResponseMessage respuesta = await ConexionApi.ApiCliente.PostAsJsonAsync(path, artista))
            {
                if (respuesta.IsSuccessStatusCode)
                {
                    artistaRecuperado = await respuesta.Content.ReadAsAsync<Artista>();
                    return artistaRecuperado;
                }
                else
                {
                    dynamic error = await respuesta.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new Exception(mensaje);
                }

            }
        }
        public static async Task<List<Artista>> ObtenerArtistaPorNombre(String nombreArtista)
        {
            string path ="Artista/"+nombreArtista;
            List<Artista> listaArtistas;
            using (HttpResponseMessage response = await ConexionApi.ApiCliente.GetAsync(path))
            {
                if (response.IsSuccessStatusCode)
                {
                    listaArtistas = await response.Content.ReadAsAsync<List<Artista>>();
                    return listaArtistas;
                }
                else
                {
                    dynamic error = await response.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new Exception(mensaje);
                }

            }
        }
        public static async Task<Artista> ObtenerArtistaPorId(String idArtista)
        {
            string path = "Artista/Id/" + idArtista;
            Artista artista;
            using (HttpResponseMessage response = await ConexionApi.ApiCliente.GetAsync(path))
            {
                if (response.IsSuccessStatusCode)
                {
                    artista = await response.Content.ReadAsAsync<Artista>();
                    return artista;
                }
                else
                {
                    dynamic error = await response.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new Exception(mensaje);
                }

            }
        }

        public static async Task<List<Artista>> ObtenerArtistaHome()
        {
            string path = "ArtistaHome";
            List<Artista> listaArtistas;
            using (HttpResponseMessage response = await ConexionApi.ApiCliente.GetAsync(path))
            {
                if (response.IsSuccessStatusCode)
                {
                    listaArtistas = await response.Content.ReadAsAsync<List<Artista>>();
                    return listaArtistas;
                }
                else
                {
                    dynamic error = await response.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new Exception(mensaje);
                }

            }
        }

        public static async Task<BitmapImage> ObtenerImagenArtista(string nombreImagen)
        {
            string path = "Artista/imagen/" + nombreImagen;
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
