using Cliente_MusiCloud.cuenta.Dominio;

namespace Cliente_MusiCloud.utilidades
{
    class SingletonCuenta
    {
        private static Cuentas cuentaSesion = null;


        private SingletonCuenta() { }

        public static void SetCuenta(Cuentas cuenta)
        {
          cuentaSesion = cuenta;
        }

        public static Cuentas GetSingletonCuenta()
        {
            return cuentaSesion;
        }

        public static void CleanSingleton()
        {
            cuentaSesion = null;
        }
    }
}
