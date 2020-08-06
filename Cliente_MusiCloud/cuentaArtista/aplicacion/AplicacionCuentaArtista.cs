using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
    }
}
