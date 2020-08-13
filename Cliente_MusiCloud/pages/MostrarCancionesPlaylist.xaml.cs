using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.cancion.aplicacion;
using Cliente_MusiCloud.cancion.dominio;
using Cliente_MusiCloud.cuenta.Dominio;
using Cliente_MusiCloud.playlist.dominio;
using Cliente_MusiCloud.playlistCanciones.aplicacion;
using Cliente_MusiCloud.playlistCanciones.dominio;
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
    /// Lógica de interacción para MostrarCancionesPlaylist.xaml
    /// </summary>
    public partial class MostrarCancionesPlaylist : Page
    {
        Playlist playlist;
        Cuentas cuenta = SingletonCuenta.GetSingletonCuenta();
        List<Cancion> listaCanciones;
        List<PlaylistCanciones> listaPlaylistCanciones;
        public MostrarCancionesPlaylist(Playlist playlistRecibida)
        {
            InitializeComponent();
            this.playlist = playlistRecibida;
            CargarInformacionPlaylist();
            CargarCancionesPlaylistAsync();
            this.listaCanciones = new List<Cancion>();
            Btn_RegresarAHome.Visibility = Visibility.Hidden;
            
        }
        public MostrarCancionesPlaylist(Playlist playlist, int tipoPlaylist)
        {
            InitializeComponent();
            this.playlist = playlist;
            CargarInformacionPlaylist();
            CargarCancionesPlaylistAsync();
            this.listaCanciones = new List<Cancion>();
            Btn_Regresar.Visibility = Visibility.Hidden;
        }

        private async void CargarCancionesPlaylistAsync()
        {
            try
            {
                listaPlaylistCanciones = await ObtenerPlaylistCanciones();
                if (listaPlaylistCanciones!=null)
                {
                    foreach (var playlistCancionDeLista in listaPlaylistCanciones)
                    {
                        Cancion cancionObtenida = await AplicacionCancion.ObtenerCancionPorId(playlistCancionDeLista.idCancion);
                        listaCanciones.Add(cancionObtenida);
                    }
                    foreach (var cancionDeLista in listaCanciones)
                    {
                        cancionDeLista.imagenPortadaCancion = await AplicacionAlbum.ObtenerImagenAlbum(cancionDeLista.portada);
                    }
                    listView_Canciones.ItemsSource = listaCanciones;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private async Task<List<PlaylistCanciones>> ObtenerPlaylistCanciones()
        {
            try
            {
                List<PlaylistCanciones> lista = await AplicacionPlaylistCanciones.ObtenerPlaylistCanciones(playlist.idPlaylist);
                return lista;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return null;
        }

        private void CargarInformacionPlaylist()
        {
            txt_NombreCreador.Text = cuenta.nombreUsuario;
            txt_NombrePlaylist.Text = playlist.nombre;
            txt_TipoPlaylist.Text = ObtenerTipoPlaylist();
            portadaAlbum.Source = playlist.imagenPortada;
        }
        private string ObtenerTipoPlaylist()
        {
            if (playlist.publica)
            {
                return "Publica";
            }
            return "Privada";
        }
        private void Btn_Regresar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Biblioteca());
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
            Button button = sender as Button;
            Cancion cancion = button.DataContext as Cancion;
            VentanaFlotante ventanaflotante = new VentanaFlotante(new AgregarCancionPlaylist(cancion));
            ventanaflotante.ShowDialog();
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

        private void Btn_RegresarAHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Home());
        }
    }
}

