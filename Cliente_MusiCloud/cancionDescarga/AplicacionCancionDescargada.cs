using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cliente_MusiCloud.cancionDescarga
{
    class AplicacionCancionDescargada
    {
        public static async Task<bool> AgregarCancionDescargada(string idCancion, string idCuenta)
        {
            string path = "CancionDescargada/" + idCancion +"/"+ idCuenta;
            using (HttpResponseMessage respuesta = await ConexionApi.ApiCliente.PostAsync(path,null))
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
    }
}
