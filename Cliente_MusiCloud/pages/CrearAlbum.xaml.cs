using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.album.dominio;
using Cliente_MusiCloud.artista.Dominio;
using Cliente_MusiCloud.cancion.dominio;
using Cliente_MusiCloud.utilidades;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cliente_MusiCloud.pages
{
    /// <summary>
    /// Lógica de interacción para CrearAlbum.xaml
    /// </summary>
    public partial class CrearAlbum : Page
    {
        List<Cancion> listaDeCanciones;
        String pathAbsolutoImagen;
        Artista artista = SingletonArtista.GetArtista();
        Album albumRecuperado;
        public CrearAlbum()
        {
            InitializeComponent();
            this.listaDeCanciones = new List<Cancion>();
            txt_NombreArchivo.IsEnabled = false;
        }

        private async void GuardarAlbum_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCamposAlbum())
            {
                if (validarAlMenosUnaCancionAñadida())
                {
                    albumRecuperado = await GuardarAlbumAsync();
                    if (albumRecuperado!=null)
                    {

                    }
                }
            }
        }
        private bool ValidarCamposAlbum()
        {
            if (String.IsNullOrEmpty(txt_NombreAlbum.Text) || String.IsNullOrEmpty(txt_CompaniaAlbum.Text))
            {
                MessageBox.Show("Favor de ingresar información dek álbum", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        private bool validarAlMenosUnaCancionAñadida()
        {
            if (listaDeCanciones.Count>0)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Debe agregar al menos una canción", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
        }
        private async Task<Album> GuardarAlbumAsync()
        {
            Album album = ObtenerDatosAlbum();
            Album albumRecuperado = null;
            try
            {
                albumRecuperado = await AplicacionAlbum.GuardarAlbum(album);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return albumRecuperado;
        }
        private Album ObtenerDatosAlbum()
        {
            Album album = new Album
            {
                nombre = txt_NombreAlbum.Text,
                compania = txt_CompaniaAlbum.Text,
                idArtista = artista.idArtista,
                idGenero = artista.idGenero,
                portada = ObtenerPortada(),
                fechaRegistro = DateTime.Now
            };
            return album;
        }
        private String ObtenerPortada()
        {
            string portadaArtista;
            if (pathAbsolutoImagen != null)
            {
                return portadaArtista = CodificacionImagenes.CodificarBase64(pathAbsolutoImagen);
            }
            return portadaArtista = "";
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
        private void btn_AgregarALista_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampoCancion())
            {
                AgregarCanciones();
                LimpiarCampoCancion();
            }
            else
            {
                MessageBox.Show("Favor de ingresar los datos de la canción", "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        private void AgregarCanciones()
        {
            Cancion cancion = new Cancion
            {
                nombre = txt_NombreCancion.Text
            };
            listaDeCanciones.Add(cancion);
            ActualizarTabla();
        }

        private void ActualizarTabla()
        {
            listViewCanciones.ItemsSource = null;
            listViewCanciones.ItemsSource = listaDeCanciones;
        }
        private void LimpiarCampoCancion()
        {
            txt_NombreCancion.Text = String.Empty;
            txt_NombreArchivo.Text = String.Empty;
        }
       
        private bool ValidarCampoCancion()
        {
            if (String.IsNullOrEmpty(txt_NombreCancion.Text) || String.IsNullOrEmpty(txt_NombreArchivo.Text))
            {
                return false;
            }
            return true;
        }
   
   
      
     
        private void btn_EliminarDeLista_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarSeleccionCancion())
            {
                Cancion cancionSeleccionada = (Cancion)listViewCanciones.SelectedItem;
                listaDeCanciones.Remove(cancionSeleccionada);
                ActualizarTabla();
            }
            else
            {
                MessageBox.Show("Selecciona una canción de la lista", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
          
        }
        private bool ValidarSeleccionCancion()
        {
            if (listViewCanciones.SelectedValue == null)
            {
                return false;
            }
            return true;
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
                    string imagen = openFileDialog.SafeFileName;
                    txt_NombreArchivo.Text = imagen;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Btn_Regresar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GestionArtista());
        }

    }
}
