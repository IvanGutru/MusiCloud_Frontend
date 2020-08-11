using Cliente_MusiCloud.artista.Dominio;

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
