using Cliente_MusiCloud.artista.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente_MusiCloud.utilidades
{
    class SingletonArtista
    {
        private static Artista artistaCuenta;

        private SingletonArtista() { }

        public static void SetArtista(Artista artista)
        {
            artistaCuenta = artista;
        }
        public static Artista GetArtista()
        {
            return artistaCuenta;
        }
    }
}
