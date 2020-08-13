using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.cancion.aplicacion;
using Cliente_MusiCloud.cancion.dominio;
using Cliente_MusiCloud.cuenta.Dominio;
using Cliente_MusiCloud.historial.aplicacion;
using Cliente_MusiCloud.historial.dominio;
using Cliente_MusiCloud.playlist.aplicacion;
using Cliente_MusiCloud.playlist.dominio;
using Cliente_MusiCloud.reproductor;
using Cliente_MusiCloud.utilidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Cliente_MusiCloud.pages
{
    /// <summary>
    /// Lógica de interacción para Biblioteca.xaml
    /// </summary>
    public partial class Biblioteca : Page
    {
        Cuentas cuenta = SingletonCuenta.GetSingletonCuenta();
        private List<Playlist> listaPlaylistUsuario;
        private List<Historial> listaHistorial;
        private List<Cancion> listaCancionesHistorial;
        private const int BIBLIOTECAPROPIA = 5;

        public Biblioteca()
        {
            InitializeComponent();
            CargarPlaylistUsuario();
            CargarColaReproduccion();
            CargarHistorialAsync();
            this.listaCancionesHistorial = new List<Cancion>();
        }

        private async void CargarPlaylistUsuario()
        {
            try
            {
                listaPlaylistUsuario = await AplicacionPlaylist.ObtenerPlaylistDeUsuario(cuenta.idCuenta);
                foreach (var playlistDelista in listaPlaylistUsuario)
                {
                    playlistDelista.imagenPortada = await AplicacionPlaylist.ObtenerImagenPlaylist(playlistDelista.portada);
                }
                listViewMisPlaylist.ItemsSource = listaPlaylistUsuario;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private async void CargarHistorialAsync()
        {
            List<Historial> listaReproduccion = await ObtenerHistorialReproduccion();
            if (listaReproduccion != null)
            {
                try
                {
                    foreach (var historialDeLista in listaReproduccion)
                    {
                        Cancion cancionDeHistorial = await AplicacionCancion.ObtenerCancionPorId(historialDeLista.idCancion);
                        listaCancionesHistorial.Add(cancionDeHistorial);
                    }
                    foreach (var cancionDeLista in listaCancionesHistorial)
                    {
                        cancionDeLista.imagenPortadaCancion = await AplicacionAlbum.ObtenerImagenAlbum(cancionDeLista.portada);
                    }
                    listViewHistorial.ItemsSource = listaCancionesHistorial;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }



        }

        private async Task<List<Historial>> ObtenerHistorialReproduccion()
        {
            try
            {
                listaHistorial = await AplicacionHistorial.ObtenerHistorialReproduccionCuenta(cuenta.idCuenta);
                return listaHistorial;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return null;
        }
        private void CargarColaReproduccion()
        {
            listViewColaReproduccion.ItemsSource = Reproductor.ColaCanciones;
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
            NavigationService.Refresh();
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

        private void listViewMisPlaylist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Playlist playlistSeleccionada = (Playlist)listViewMisPlaylist.SelectedItem;

            if (playlistSeleccionada.idTipoPlaylist == BIBLIOTECAPROPIA)
            {
                NavigationService.Navigate(new BibliotecaPropiaPage(playlistSeleccionada));
            }
            else
            {
                NavigationService.Navigate(new MostrarCancionesPlaylist(playlistSeleccionada));
            }
        }
    }
}
