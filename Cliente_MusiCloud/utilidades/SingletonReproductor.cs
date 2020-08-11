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
