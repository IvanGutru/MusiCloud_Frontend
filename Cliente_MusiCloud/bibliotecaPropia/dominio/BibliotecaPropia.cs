
using System;
using System.Windows.Media.Imaging;

namespace Cliente_MusiCloud.bibliotecaPropia.dominio
{
    public class BibliotecaPropia
    {
        public int idBibliotecaPropia { get; set; }
        public string nombreCancion { get; set; }
        public string generoCancion { get; set; }
        public string albumCancion { get; set; }
        public string portada { get; set; }
        public string duracion { get; set; }
        public string archivo { get; set; }
        public int idPlaylist { get; set; }
        public string idCuenta {get; set;}
        public DateTime fechaRegistro { get; set; }
        public BitmapImage portadaImagenBibliotecaPropia { get; set; }

    }
}
