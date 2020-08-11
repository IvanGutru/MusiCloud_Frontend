using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cliente_MusiCloud.cuentaArtista.dominio;

namespace Cliente_MusiCloud.cuentaArtista.aplicacion
{
    class AplicacionCuentaArtista
    {
        public static async Task<bool> RegistrarCuentaArtista(CuentaArtista cuentaArtista)
        {
            string path = "CuentaArtista";
            using (HttpResponseMessage respuesta = await ConexionApi.ApiCliente.PostAsJsonAsync(path, cuentaArtista))
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
        public static async Task<CuentaArtista> ObtenerCuentaArtistaIdCuenta(String idCuenta)
        {
            string path = "CuentaArtista/" + idCuenta;
            CuentaArtista cuentaArtista;
            using (HttpResponseMessage response = await ConexionApi.ApiCliente.GetAsync(path))
            {
                if (response.IsSuccessStatusCode)
                {
                    cuentaArtista = await response.Content.ReadAsAsync<CuentaArtista>();
                    return cuentaArtista;
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
