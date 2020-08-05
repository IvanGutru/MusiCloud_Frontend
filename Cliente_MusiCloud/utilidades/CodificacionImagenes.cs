
using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

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

        public static BitmapImage DecodificarBase64(String imagenCodificada)
        {
            BitmapImage bitmapImage = new BitmapImage(); 
            byte[] imagenEnBytes = Convert.FromBase64String(imagenCodificada);
            var ms = new MemoryStream(imagenEnBytes);
            
                //Image imagen = Image.FromStream(ms,true);
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.EndInit();
                return bitmapImage;
            
        }
    }
}
