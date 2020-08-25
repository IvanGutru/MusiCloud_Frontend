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
        /// <summary>
        /// Guarda el audio de la canción en la ruta local del ordenador
        /// </summary>
        /// <param name="cancion"> que fue seleccionada para descargarse</param>
        /// <returns> true si la canción escribió correctamente y false si ocurrió un error</returns>
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
        /// <summary>
        /// Manda a llamar la descarga del archivo y guarda la relación
        /// de la canción que se descargó en la playlist correspondiente
        /// </summary>
        /// <param name="cancion">Que se seleccionó para descargarse</param>
        /// <param name="cuenta">Qúe está descargando la canción</param>
        /// <returns>true si se guardó la relación y false si ocucrrió un error</returns>
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
        /// <summary>
        /// Valida que la canción ya ha sido descargada
        /// </summary>
        /// <param name="cancion">que se quiere descargar</param>
        /// <param name="cuenta">que quiere descargar la canción</param>
        /// <returns></returns>
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
