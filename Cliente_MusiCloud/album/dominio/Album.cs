using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string idArtista  { get; set; }

    }
}
