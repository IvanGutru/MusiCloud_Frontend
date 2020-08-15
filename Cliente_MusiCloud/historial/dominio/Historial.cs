using System;

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
