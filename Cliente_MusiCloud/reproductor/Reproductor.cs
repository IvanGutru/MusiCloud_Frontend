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

    }
}
