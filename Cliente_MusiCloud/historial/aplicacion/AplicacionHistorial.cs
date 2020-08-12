using Cliente_MusiCloud.historial.dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Cliente_MusiCloud.historial.aplicacion
{
    class AplicacionHistorial
    {
        public static async Task<List<Historial>> ObtenerHistorialReproduccionCuenta(string idCuenta)
        {
            string path = "Historial/"+idCuenta;
            List<Historial> listaHistorialReproduccion;
            using (HttpResponseMessage respuesta = await ConexionApi.ApiCliente.GetAsync(path))
            {
                if (respuesta.IsSuccessStatusCode)
                {
                    listaHistorialReproduccion = await respuesta.Content.ReadAsAsync<List<Historial>>();
                    return listaHistorialReproduccion;
                }
                else
                {
                    dynamic error = await respuesta.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new FormatException(mensaje);
                }
            }
        }

        public static async Task<bool> AñadirCancionAHistorial(string idCancion,string idCuenta)
        {
            string path = "Cancion/Historial/" +idCancion+"/"+ idCuenta;
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
