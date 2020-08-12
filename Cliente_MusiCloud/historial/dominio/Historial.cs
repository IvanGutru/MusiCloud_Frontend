using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente_MusiCloud.historial.dominio
{
    class Historial
    {
        public int idHistorial { get; set; }
        public DateTime fechaReproduccion { get; set; }
        public string idCuenta { get; set; }
        public string idCancion { get; set; }
    }
}
