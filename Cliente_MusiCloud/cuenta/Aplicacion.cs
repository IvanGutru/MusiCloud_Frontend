using Cliente_MusiCloud.cuenta.Dominio;
using Cliente_MusiCloud.cuenta.LoginRR;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cliente_MusiCloud.Cuenta
{
    class Aplicacion
    {
        public static async Task<Cuentas> Login(LoginRequest request)
        {
            string path = "Cuenta/Login";
            Cuentas loginResponse = null;
            using (HttpResponseMessage response = await ConexionApi.ApiCliente.PostAsJsonAsync(path, request))
            {
                if (response.IsSuccessStatusCode)
                {
                    loginResponse = await response.Content.ReadAsAsync<Cuentas>();
                    return loginResponse;
                }
                else
                {
                    dynamic error = await response.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new Exception(mensaje);
                }

            }
        }
        public static async Task<bool> CrearCuenta(Cuentas cuenta)
        {
            string path = "Cuenta";
            using (HttpResponseMessage response = await ConexionApi.ApiCliente.PostAsJsonAsync(path,cuenta))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    dynamic error = await response.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new Exception(mensaje);

                }
            }

        }
        public static async Task<String> ConvertirseEnCreadorDeContenido(string idCuenta )
        {
            string path = "Cuenta/CreadorContenido/"+idCuenta;
            using (HttpResponseMessage response = await ConexionApi.ApiCliente.PutAsJsonAsync(path, idCuenta))
            {
                if (response.IsSuccessStatusCode)
                {
                    string respuesta = await response.Content.ReadAsStringAsync();
                    return respuesta;
                }
                else
                {
                    dynamic error = await response.Content.ReadAsAsync<dynamic>();
                    string mensaje = error.error;
                    throw new Exception(mensaje);

                }
            }

        }
        public static async Task<bool> ActualizarCuenta(Cuentas cuenta)
        {
            string path = "Cuenta/Actualizar";
            using (HttpResponseMessage respuesta = await ConexionApi.ApiCliente.PutAsJsonAsync(path, cuenta))
            {
                if (respuesta.IsSuccessStatusCode)
                {
                    bool mensaje = true;
                    return mensaje;
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
