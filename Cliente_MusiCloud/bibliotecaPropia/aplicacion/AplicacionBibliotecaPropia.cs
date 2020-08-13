using Cliente_MusiCloud.bibliotecaPropia.dominio;
using Cliente_MusiCloud.utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Cliente_MusiCloud.bibliotecaPropia.aplicacion
{
    class AplicacionBibliotecaPropia
    {
        public static async Task<BibliotecaPropia> GuardarBibliotecaPropia(BibliotecaPropia bibliotecaPropiaRecibida)
        {
            BibliotecaPropia bibliotecaRegistrada;
            string path = "BibliotecaPropia";
            using (HttpResponseMessage respuesta = await ConexionApi.ApiCliente.PostAsJsonAsync(path, bibliotecaPropiaRecibida))
            {
                if (respuesta.IsSuccessStatusCode)
                {
                    bibliotecaRegistrada = await respuesta.Content.ReadAsAsync<BibliotecaPropia>();
                    return bibliotecaRegistrada;
                }
                else
                {
                    dynamic error = await respuesta.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new FormatException(mensaje);
                }
            }
        }

        public static async Task<List<BibliotecaPropia>> ObtenerBibliotecaPropia(int idPlaylist, string idCuenta)
        {
            List<BibliotecaPropia> listaBibliotecaRegistrada;
            string path = "BibliotecaPropia/"+idPlaylist+"/"+idCuenta;
            using (HttpResponseMessage respuesta = await ConexionApi.ApiCliente.GetAsync(path))
            {
                if (respuesta.IsSuccessStatusCode)
                {
                    listaBibliotecaRegistrada = await respuesta.Content.ReadAsAsync<List<BibliotecaPropia>>();
                    return listaBibliotecaRegistrada;
                }
                else
                {
                    dynamic error = await respuesta.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new FormatException(mensaje);
                }
            }
        }
        public static async Task<BitmapImage> ObtenerImagenBibliotecaPropia(string nombreImagen)
        {
            string path = "BibliotecaPropia/" + nombreImagen;
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
