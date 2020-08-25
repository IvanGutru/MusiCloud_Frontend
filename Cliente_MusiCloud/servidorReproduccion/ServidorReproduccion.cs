using System;
using Thrift.Protocol;
using Thrift.Transport;
using Thrift.Transport.Client;

namespace Cliente_MusiCloud.ServidorReproduccion
{
    class ServidorReproduccion
    {
        public static ServicioReproduccion.Client client;

        public ServidorReproduccion() { }
        /// <summary>
        /// Inicializa la conexión con nuestro servidor de reproducción
        /// </summary>
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
        /// <summary>
        /// Cierra la conexión con nuestro servidor de reproducción
        /// </summary>
        public static void Desconectar()
        {
            try
            {
                client = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
