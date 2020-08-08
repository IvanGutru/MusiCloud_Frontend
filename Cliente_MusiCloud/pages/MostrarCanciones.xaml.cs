using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.album.dominio;
using Cliente_MusiCloud.cancion.aplicacion;
using Cliente_MusiCloud.cancion.dominio;
using Cliente_MusiCloud.reproductor;
using Cliente_MusiCloud.utilidades;
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
    /// Lógica de interacción para MostrarCanciones.xaml
    /// </summary>
    public partial class MostrarCanciones : Page
    {
        Album album;
        List<Cancion> listaCanciones;
        String idAlbum;
        public MostrarCanciones(Album albumLista)
        {
            album = albumLista;
            InitializeComponent();
            txt_NombreAlbum.Text = album.nombre;
            txt_NombreCompania.Text = album.compania;
            CargarCanciones();
            Btn_RegresarAMostrarAlbumes.Visibility = Visibility.Hidden;

        }
        public MostrarCanciones(String idAlbum)
        {
            InitializeComponent();
            this.idAlbum = idAlbum;
            CargarCancionesDesdeMostrarAlbum();
            Btn_Regresar.Visibility = Visibility.Hidden;
            CargarInformacionAlbum();
        }
        private async void CargarCanciones()
        {
            if (album != null)
            {
                try
                {
                    listaCanciones = await AplicacionCancion.ObtenerCancionesPorIdAlbumAsync(album.idAlbum);
                    listView_Canciones.ItemsSource = listaCanciones;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private async void CargarCancionesDesdeMostrarAlbum()
        {
            try
            {
                listaCanciones = await AplicacionCancion.ObtenerCancionesPorIdAlbumAsync(idAlbum);
                listView_Canciones.ItemsSource = listaCanciones;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async void CargarInformacionAlbum()
        {
            try
            {
                List<Album> album = await AplicacionAlbum.ObtenerAlbumPorId(idAlbum);
                foreach (var nombre in album)
                {
                    txt_NombreAlbum.Text = nombre.nombre;
                    txt_NombreCompania.Text = nombre.compania; 
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Regresar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MostrarArtistas());
        }
        private void Btn_Regresar_MostrarCanciones_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MostrarAlbumes());
        }
        private async void btn_Reproducir_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Cancion cancion = button.DataContext as Cancion;
            await Reproductor.Reproducir(cancion);
            SingletonReproductor.GetPaginaPrincipal().CargarInformacion(cancion);
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
            Button button = sender as Button;
            Cancion cancion = button.DataContext as Cancion;
            //GenerarRadio(cancion);
        }
    }
}
