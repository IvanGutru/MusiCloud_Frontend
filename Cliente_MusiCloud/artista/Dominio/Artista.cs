using System;
using System.Windows.Media.Imaging;

namespace Cliente_MusiCloud.artista.Dominio
{
    public class Artista
    {
        public string idArtista { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string portada { get; set; }
        public DateTime fechaRegistro { get; set; }
        public int idGenero { get; set; }

        public BitmapImage imagenPortadaArtista { get; set; }


    }
}
