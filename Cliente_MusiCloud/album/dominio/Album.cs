using Cliente_MusiCloud.artista.Dominio;
using System;
using System.Windows.Media.Imaging;

namespace Cliente_MusiCloud.album.dominio
{
    public class Album
    {
        public string idAlbum { get; set; }
        public string nombre { get; set; }
        public string compania { get; set; }
        public string portada { get; set; }
        public int idGenero  { get; set; }
        public DateTime fechaRegistro { get; set; }
        public string fechalanzamiento { get; set; }
        public string idArtista  { get; set; }
        public BitmapImage imagenPortadaAlbum { get; set; }
        public Artista artista { get; set; }

    
    }
}
