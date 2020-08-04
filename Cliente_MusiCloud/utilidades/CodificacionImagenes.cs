
using System;
using System.Drawing;
using System.IO;

namespace Cliente_MusiCloud.utilidades
{
    public class CodificacionImagenes
    {
        public static string CodificarBase64(string rutaImagen)
        {
            using (Image imagen = Image.FromFile(rutaImagen))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    imagen.Save(m, imagen.RawFormat);
                    byte[] imagenEnBytes = m.ToArray();
                    string base64String = Convert.ToBase64String(imagenEnBytes);
                    return base64String;
                }
            }
        }
    }
}
