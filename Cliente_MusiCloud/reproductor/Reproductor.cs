using Cliente_MusiCloud.cancion.dominio;
using Cliente_MusiCloud.historial.aplicacion;
using Cliente_MusiCloud.utilidades;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Cliente_MusiCloud.reproductor
{
    class Reproductor
    {
        private const string PATH = "C:/Users/IvanGutru/Documents/0.-IngenieriaSoftware/SextoSemestre/";
        public static WaveOutEvent waveOutEvent { set; get; }
        public static WaveStream waveStream { set; get; }
        public static bool cancionLista { get; set; }
        public static Queue<Cancion> ColaCanciones { get; set; }

        
        public Reproductor() { }

        public static void Initialize()
        {
            waveOutEvent = new WaveOutEvent();
            cancionLista = false;
            ColaCanciones = new Queue<Cancion>();
            ServidorReproduccion.ServidorReproduccion.Conectar(); 
        }
        /// <summary>
        /// Obtiene el archivo de audio del servidor de reproducción
        /// E inicia la reproducción del archivo mp3
        /// </summary>
        /// <param name="cancion"> canción que se seleccionó para reproducirse</param>
        /// <returns> true si la canción comenzó a reproducirse y false si ocurrió un error</returns>
        public static async Task<bool> Reproducir(Cancion cancion)
        {
            try
            {
                var audioCancion = await ServidorReproduccion.ServidorReproduccion.client.ObtenerCancionAsync(cancion.archivo);
                Reproductor.PararReproduccion();
                Mp3FileReader mp3Reader = new Mp3FileReader(new MemoryStream(audioCancion.Audio));
                waveStream = new WaveChannel32(mp3Reader);
                waveOutEvent.Init(waveStream);
                cancionLista = true;
                await AplicacionHistorial.AñadirCancionAHistorial(cancion.idCancion,SingletonCuenta.GetSingletonCuenta().idCuenta);
                Reproductor.ComenzarReproduccion();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        /// <summary>
        /// Obtiene el archivo que se ha guardado localmente para su reproducción
        /// </summary>
        /// <param name="cancion"> Que ha sido seleccionada para reproducirse</param>
        /// <returns>true si la cnación comeó a reproducirse y fasle si oucrrió un error</returns>
        public static async Task<bool> ReproducirOffline(Cancion cancion)
        {
            try
            {
                var audioCancion = File.ReadAllBytes(PATH + cancion.nombre+".mp3");
                Reproductor.PararReproduccion();
                Mp3FileReader mp3Reader = new Mp3FileReader(new MemoryStream(audioCancion));
                waveStream = new WaveChannel32(mp3Reader);
                waveOutEvent.Init(waveStream);
                cancionLista = true;
                await AplicacionHistorial.AñadirCancionAHistorial(cancion.idCancion, SingletonCuenta.GetSingletonCuenta().idCuenta);
                Reproductor.ComenzarReproduccion();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

        }
        public static bool ComenzarReproduccion()
        {
            if (cancionLista)
            {
                waveOutEvent.Play();
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool PararReproduccion()
        {
            if (cancionLista)
            {
                waveOutEvent.Stop();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool EstaReproduciendose()
        {
            if (waveOutEvent.PlaybackState == PlaybackState.Playing)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static double ObtenerTotalSegundosCancion()
        {
            return waveStream.TotalTime.TotalSeconds;
        }

        public static double ObtenerSegundosActuales()
        {
            return waveStream.CurrentTime.TotalSeconds;
        }
        public static double ObtenerTiempoParaBarra()
        {
            return (waveStream.CurrentTime.TotalSeconds * 100) / waveStream.TotalTime.TotalSeconds;
        }
        public static bool TerminoCancion()
        {
            if (waveStream.Position >= waveStream.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<Cancion> ReproducirSiguienteCancion()
        {
            if (ColaCanciones.Count > 0)
            {
                PararReproduccion();
                Cancion cancion = ColaCanciones.Dequeue();
                cancionLista = false;
                if (ValidarConexionCliente())
                {
                    if (await Reproducir(cancion))
                        return cancion;
                    return null;
                }
                else
                {
                    if (await ReproducirOffline(cancion))
                        return cancion;
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static void ReiniciarCancion()
        {
            if (cancionLista)
            {
                waveStream.Position = 0;

            }
        }

        public static void ActualizarVolumen(double volume)
        {
            if (waveOutEvent != null)
            {
                waveOutEvent.Volume = (Convert.ToSingle(volume)) / 100f;

            }
        }
        public static void ActualizarPosicionCancion(double position)
        {
            var totalSeconds = (position * waveStream.TotalTime.TotalSeconds) / 100;

            waveStream.CurrentTime = TimeSpan.FromSeconds(totalSeconds);
        }
        public static void AgregarCancionACola(Cancion cancion)
        {
            ColaCanciones.Enqueue(cancion);
        }
        public static void AgregarSiguienteACola(Cancion cancion)
        {
            List<Cancion> listaCanciones = ColaCanciones.Prepend(cancion).ToList();
            ColaCanciones.Clear();
            foreach (var cancionlista in listaCanciones)
            {
                ColaCanciones.Enqueue(cancionlista);
            }

        }
        public static void AgregarListaCancionesACola(List<Cancion> cancion)
        {
            foreach (var item in cancion)
            {
                ColaCanciones.Enqueue(item);
            }
        }

        public static bool ValidarConexionCliente()
        {
            if (ServidorReproduccion.ServidorReproduccion.client !=null)
            {
                return true;
            }
            return false;
        }

    }
}
