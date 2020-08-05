using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Cliente_MusiCloud.playlist.dominio
{
    public class Playlist
    {
        public int idPlaylist { get; set; }
        public string nombre { get; set; }
        public bool publica { get; set; }
        public DateTime fechaCreacion { get; set; }
        public string portada { get; set; }
        public string idCuenta { get; set; }
        public int idTipoPlaylist { get; set; }
        public BitmapImage imagenPortada { get; set; }
    }
}
