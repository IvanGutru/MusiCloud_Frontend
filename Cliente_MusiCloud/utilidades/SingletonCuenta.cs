using Cliente_MusiCloud.cuenta.LoginRR;

namespace Cliente_MusiCloud.utilidades
{
    class SingletonCuenta
    {
        private static LoginResponse cuentaSesion = null;

        private SingletonCuenta() { }

        public static void SetCuenta(LoginResponse cuenta)
        {
            if (cuentaSesion == null)
            {
                cuentaSesion = cuenta;
            }
        }

        public static LoginResponse GetSingletonCuenta()
        {
            return cuentaSesion;
        }

        public static void CleanSingleton()
        {
            cuentaSesion = null;
        }
    }
}
