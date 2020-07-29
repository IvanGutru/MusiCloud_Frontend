using Cliente_MusiCloud.cuenta.Dominio;
using Cliente_MusiCloud.cuenta.LoginRR;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cliente_MusiCloud.Cuenta
{
    class Aplicacion
    {
        public static async Task<LoginResponse> Login(LoginRequest request)
        {
            string path = "Cuenta/Login";
            LoginResponse loginResponse = null;
            using (HttpResponseMessage response = await ConexionApi.ApiCliente.PostAsJsonAsync(path, request))
            {
                if (response.IsSuccessStatusCode)
                {
                    loginResponse = await response.Content.ReadAsAsync<LoginResponse>();
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
                    //String respuesta = await response.Content.ReadAsAsync<String>();
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
    }
}
