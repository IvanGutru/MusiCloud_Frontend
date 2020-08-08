using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente_MusiCloud.utilidades
{
    class SingletonReproductor
    {
        private static PaginaPrincipal paginaPrincipal;

        private SingletonReproductor() { }

        public static void SetPaginaPrincipal(PaginaPrincipal paginaP)
        {
            paginaPrincipal = paginaP;
        }
        public static PaginaPrincipal GetPaginaPrincipal()
        {
            return paginaPrincipal;
        }
    }
}
