using Cliente_MusiCloud.cancion.dominio;
using Cliente_MusiCloud.cuenta.Dominio;
using Cliente_MusiCloud.playlist.aplicacion;
using Cliente_MusiCloud.reproductor;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Cliente_MusiCloud.descargar
{
    class DescargarCancion
    {
        private const string PATH = "C:/Users/IvanGutru/Documents/0.-IngenieriaSoftware/SextoSemestre/";
        public DescargarCancion() { }

        public static async Task<bool> GuardarCancion(Cancion cancion)
        {
            try
            {
                var cancionAudio = await ServidorReproduccion.ServidorReproduccion.client.ObtenerCancionAsync(cancion.archivo);
                File.WriteAllBytes(PATH + cancion.nombre + ".mp3", cancionAudio.Audio);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
         
        }

        public static async Task<bool> Descargar(Cancion cancion, Cuentas cuenta)
        {
            try
            {
                if (Reproductor.ValidarConexionCliente())
                {
                    if (await GuardarCancion(cancion))
                    {
                        if (await AplicacionPlaylist.AgregarADescargas(cancion.idCancion, cuenta.idCuenta))
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return false;
        }

        public static async Task<bool> ValidarCancionDescargada(Cancion cancion, Cuentas cuenta)
        {
            try
            {
                bool estado = await AplicacionPlaylist.ValidarCancionEnDescargas(cancion.idCancion, cuenta.idCuenta);
                return estado;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
    }
}
