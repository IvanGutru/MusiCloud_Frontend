using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Protocol;
using Thrift.Transport;
using Thrift.Transport.Client;

namespace Cliente_MusiCloud.ServidorReproduccion
{
    class ServidorReproduccion
    {
        public static ServicioReproduccion.Client client;

        public ServidorReproduccion() {}

        public static void Conectar()
        {
            try
            {
                TTransport transport = new TSocketTransport("localhost", 8000);
                TProtocol protocol = new TBinaryProtocol(transport);
                client = new ServicioReproduccion.Client(protocol);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
    }
}
