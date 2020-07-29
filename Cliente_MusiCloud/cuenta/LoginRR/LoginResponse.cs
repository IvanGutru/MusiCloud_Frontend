using Cliente_MusiCloud.cuenta.Dominio;
using System;

namespace Cliente_MusiCloud.cuenta.LoginRR
{
    class LoginResponse
    {
        public Cuentas cuenta { get; set; }
        public String token { get; set; }
    }
}
