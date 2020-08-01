using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente_MusiCloud.artista.Dominio
{
    class Artista
    {
        public string idArtista { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string portada { get; set; }
        public DateTime fechaRegistro { get; set; }
        public int idGenero { get; set; }
    }
}
