using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.album.dominio;
using Cliente_MusiCloud.cancion.aplicacion;
using Cliente_MusiCloud.cancion.dominio;
using Cliente_MusiCloud.reproductor;
using Cliente_MusiCloud.utilidades;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Cliente_MusiCloud.pages
{
    /// <summary>
    /// Lógica de interacción para MostrarAlbumes.xaml
    /// </summary>
    public partial class MostrarAlbumes : Page
    {
        List<Cancion> listaCanciones;

        public MostrarAlbumes()
        {
            InitializeComponent();
        }

        private async void Btn_Buscar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampoVacio())
            {
                try
                {
                    string nombreAlbum = txt_NombreAlbum.Text;
                    List<Album> listaAlbumes = await AplicacionAlbum.ObtenerAlbumPorNombre(nombreAlbum);
                    foreach (var albumes in listaAlbumes)
                    {
                        albumes.imagenPortadaAlbum = await AplicacionAlbum.ObtenerImagenAlbum(albumes.portada);
                        albumes.fechalanzamiento = albumes.fechaRegistro.ToShortDateString();
                        
                    }
                    listViewAlbumes.ItemsSource = listaAlbumes;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
   
        }

        private bool ValidarCampoVacio()
        {
            if (String.IsNullOrEmpty(txt_NombreAlbum.Text))
            {
                return false;
            }
            return true;
        }
   

        private void listViewAlbumes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Album album = (Album)listViewAlbumes.SelectedItem;
            if (listViewAlbumes.SelectedItems.Count>0)
            {
                CargarCancionesDeAlbum(album);
            }
        }

        private async void CargarCancionesDeAlbum(Album albumRecibido)
        {
            try
            {
                listaCanciones = await AplicacionCancion.ObtenerCancionesPorIdAlbumAsync(albumRecibido.idAlbum);
                foreach (var cancion in listaCanciones)
                {
                    cancion.imagenPortadaCancion = await AplicacionAlbum.ObtenerImagenAlbum(cancion.portada);
                }
                listView_Canciones.ItemsSource = listaCanciones;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private async void btn_Reproducir_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Cancion cancion = button.DataContext as Cancion;
            await Reproductor.Reproducir(cancion);
            SingletonReproductor.GetPaginaPrincipal().CargarInformacionAsync(cancion);
        }


        private void btn_agregarCola_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Cancion cancion = button.DataContext as Cancion;
            Reproductor.AgregarCancionACola(cancion);
        }

        private void btn_agregarSiguiente_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Cancion cancion = button.DataContext as Cancion;
            Reproductor.AgregarSiguienteACola(cancion);
        }

        private void btn_agregarAPlaylist_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_generarRadio_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_AgregarTodasLasCanciones_Click(object sender, RoutedEventArgs e)
        {
            Reproductor.ColaCanciones.Clear();
            Reproductor.AgregarListaCancionesACola(listaCanciones);
            SingletonReproductor.GetPaginaPrincipal().SiguienteCancion();
        }
    }
}
