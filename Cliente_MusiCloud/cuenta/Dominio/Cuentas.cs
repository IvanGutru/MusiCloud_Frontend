using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente_MusiCloud.cuenta.Dominio
{
    public class Cuentas
    {
        public string idCuenta { get; set; }
        public string correo { get; set; }
        public string contraseña { get; set; }
        public string apellidos { get; set; }
        public string nombreUsuario { get; set; }
        public string nombre { get; set; }
        public bool creadorContenido { get; set; }

        public Cuentas()
        {
        }
    }
}
