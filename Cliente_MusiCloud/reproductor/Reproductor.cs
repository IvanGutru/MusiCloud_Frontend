using Cliente_MusiCloud.cancion.dominio;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente_MusiCloud.reproductor
{
    class Reproductor
    {
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

        public static async Task<bool> Reproducir(Cancion cancion)
        {
            try
            {
                var trackAudio = await ServidorReproduccion.ServidorReproduccion.client.ObtenerCancionAsync(cancion.archivo);
                PararReproduccion();
                Mp3FileReader mp3Reader = new Mp3FileReader(new MemoryStream(trackAudio.Audio));
                waveStream = new WaveChannel32(mp3Reader);
                waveOutEvent.Init(waveStream);
                cancionLista = true;
                ComenzarReproduccion();
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
                Reproductor.PararReproduccion();
                Cancion cancion = ColaCanciones.Dequeue();
                cancionLista = false;
                if (await Reproductor.Reproducir(cancion))
                {
                    return cancion;
                }
                else
                {
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
            foreach (var track in listaCanciones)
            {
                ColaCanciones.Enqueue(track);
            }

        }
        public static void AgregarListaCancionesACola(List<Cancion> cancion)
        {
            foreach (var item in cancion)
            {
                ColaCanciones.Enqueue(item);
            }
        }

    }
}
