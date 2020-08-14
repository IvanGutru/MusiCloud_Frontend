using Cliente_MusiCloud.album.dominio;
using Cliente_MusiCloud.genero.dominio;
using System;
using System.Windows.Media.Imaging;

namespace Cliente_MusiCloud.cancion.dominio
{
    public class Cancion
    {
        public String idCancion { get; set; }
        public String nombre { get; set; }
        public String duracion { get; set; }
        public String archivo { get; set; }
        public String idAlbum { get; set; }
        public String portada { get; set; }
        public BitmapImage imagenPortadaCancion { get; set; }
        public Genero genero { get; set; }
        public Album album { get; set; }
        public bool meGusta { get; set; }
    }
}
