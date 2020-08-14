using Cliente_MusiCloud.bibliotecaPropia.aplicacion;
using Cliente_MusiCloud.bibliotecaPropia.dominio;
using Cliente_MusiCloud.cuenta.Dominio;
using Cliente_MusiCloud.playlist.dominio;
using Cliente_MusiCloud.utilidades;
using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Cliente_MusiCloud.pages
{
    /// <summary>
    /// Lógica de interacción para CrearBibliotecaPropia.xaml
    /// </summary>
    public partial class CrearBibliotecaPropia : Page
    {
        Playlist playlist;
        string pathAbsolutoImagen;
        Cuentas cuenta = SingletonCuenta.GetSingletonCuenta();
        BibliotecaPropia bibliotecaPropia;
        Random random = new Random();
        public CrearBibliotecaPropia(Playlist playlistRecibida)
        {
            InitializeComponent();
            this.playlist = playlistRecibida;
        }

        private bool ValidarCampos()
        {
            if (String.IsNullOrEmpty(txt_NombreAlbum.Text) || String.IsNullOrEmpty(txt_Genero.Text) || 
                String.IsNullOrEmpty(txt_NombreCancion.Text) || String.IsNullOrEmpty(txt_NombreArchivo.Text))
            {
                return false;
            }
            return true;
        }
        private async void btn_AgregarABiblioteca_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampos())
            {
                if (await GuardaCancionServidorAsync())
                {
                    MessageBox.Show("Registro éxitoso", "Registro éxitoso", MessageBoxButton.OK,MessageBoxImage.Information);
                    LimpiarCampos();
                }

            }
            else
            {
                MessageBox.Show("Favor de ingresar información en todos los campos", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private async Task<bool> GuardaCancionServidorAsync()
        {
            bool realizado = false;
            try
            {
                string archivo = txt_NombreArchivo.Text;
                var archivoEnbytes = ObtenerBytesArchivo(archivo);
                bibliotecaPropia = await GuardarBibliotecaPropia();
                if (bibliotecaPropia!=null)
                {
                    AudioCancion audioCancion = new AudioCancion
                    {
                        NombreCancion = bibliotecaPropia.archivo,
                        Audio = archivoEnbytes
                    };
                    realizado = await ServidorReproduccion.ServidorReproduccion.client.SubirAudioAsync(audioCancion);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return realizado;
        }

        private async Task<BibliotecaPropia> GuardarBibliotecaPropia()
        {
            BibliotecaPropia bibliotecaPropia = ObtenerBibliotecaFormulario();
            BibliotecaPropia bibliotecaPropiaGuardada = null;
            try
            {
                bibliotecaPropiaGuardada = await AplicacionBibliotecaPropia.GuardarBibliotecaPropia(bibliotecaPropia);
                return bibliotecaPropiaGuardada;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return bibliotecaPropiaGuardada;
        }
        private BibliotecaPropia ObtenerBibliotecaFormulario()
        {
            string archivo = txt_NombreArchivo.Text;
            var archivoEnbytes = ObtenerBytesArchivo(archivo);
            string duracion = ObtenerDuracionCancion(archivoEnbytes);
            archivo = random.Next().ToString();
            BibliotecaPropia bibliotecaPropiaFormulario = new BibliotecaPropia
            {
                albumCancion = txt_NombreAlbum.Text,
                generoCancion = txt_Genero.Text,
                nombreCancion = txt_NombreCancion.Text,
                portada = ObtenerPortadaAlbum(),
                archivo = archivo,
                duracion = duracion,
                fechaRegistro = DateTime.Now,
                idCuenta = cuenta.idCuenta,
                idPlaylist = playlist.idPlaylist
            };
            return bibliotecaPropiaFormulario;
        }
        
        private String ObtenerPortadaAlbum()
        {
            string portadaAlbum;
            if (pathAbsolutoImagen != null)
            {
                return portadaAlbum = CodificacionImagenes.CodificarBase64(pathAbsolutoImagen);
            }
            return "";
        }
        private Byte[] ObtenerBytesArchivo(string path)
        {
            return File.ReadAllBytes(path);
        }
        private String ObtenerDuracionCancion(Byte[] bytesCancion)
        {
            Mp3FileReader mp3Reader = new Mp3FileReader(new MemoryStream(bytesCancion));
            WaveStream waveStream = new WaveChannel32(mp3Reader);
            double totalSegudos = waveStream.TotalTime.TotalSeconds;
            TimeSpan timeSpan = TimeSpan.FromSeconds(totalSegudos);
            string duracion = string.Format("{0}:{1}", timeSpan.Duration().Minutes, timeSpan.Duration().Seconds);
            return duracion;

        }
        private void LimpiarCampos()
        {
            txt_NombreAlbum.Text = string.Empty;
            txt_NombreCancion.Text = string.Empty;
            txt_Genero.Text = string.Empty;
            txt_NombreArchivo.Text = string.Empty;
        }

        private void Btn_Regresar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BibliotecaPropiaPage(playlist));
        }

        private void subirAudio_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Formato de archivos ¨(*.mp3)|*.mp3";
            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    string archivo = openFileDialog.FileName;
                    txt_NombreArchivo.Text = archivo;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void subirPortada_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Formato de archivos ¨(*.jpg, *jpeg, *.png)|*.jpg; *.jpeg; *.png";
            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    string imagen = openFileDialog.FileName;
                    pathAbsolutoImagen = imagen;
                    PortadaCancion.Source = new BitmapImage(new Uri(imagen, UriKind.Absolute));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

      
    }
}
