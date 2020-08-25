using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.album.dominio;
using Cliente_MusiCloud.cancion.aplicacion;
using Cliente_MusiCloud.cancion.dominio;
using Cliente_MusiCloud.cuenta.Dominio;
using Cliente_MusiCloud.descargar;
using Cliente_MusiCloud.genero.aplicacion;
using Cliente_MusiCloud.playlist.aplicacion;
using Cliente_MusiCloud.playlist.dominio;
using Cliente_MusiCloud.playlistCanciones.aplicacion;
using Cliente_MusiCloud.playlistCanciones.dominio;
using Cliente_MusiCloud.reproductor;
using Cliente_MusiCloud.utilidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

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
            this.listaCanciones = new List<Cancion>();
            CargarInformacionPlaylist();
            CargarCancionesPlaylistAsync();
            OcultarCamposParaHome();
        }

        private async void CargarCancionesPlaylistAsync()
        {
            try
            {
                listaCanciones = await ObtenerCancionesPlaylistAsync();
                listaCanciones = await ObtenerCancionesAlbumGeneroAsync(listaCanciones);
                listView_Canciones.ItemsSource = listaCanciones;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private async Task<List<Cancion>> ObtenerCancionesPlaylistAsync()
        {
            listaPlaylistCanciones = await ObtenerPlaylistCanciones();
            if (listaPlaylistCanciones != null)
            {
                foreach (var playlistCancionDeLista in listaPlaylistCanciones)
                {
                    Cancion cancionObtenida = await AplicacionCancion.ObtenerCancionPorId(playlistCancionDeLista.idCancion);
                    listaCanciones.Add(cancionObtenida);
                }
                foreach (var cancionDeLista in listaCanciones)
                {
                    cancionDeLista.imagenPortadaCancion = await AplicacionAlbum.ObtenerImagenAlbum(cancionDeLista.portada);
                    cancionDeLista.meGusta = await AplicacionPlaylist.ValidarCancionEnMeGusta(cancionDeLista.idCancion, cuenta.idCuenta);
                }
            }
            return listaCanciones;
        }
        private async Task<List<Cancion>> ObtenerCancionesAlbumGeneroAsync(List<Cancion> listaCanciones)
        {
            foreach (var cancionDelista in listaCanciones)
            {
                cancionDelista.album = await AplicacionAlbum.ObtenerAlbumPorId(cancionDelista.idAlbum);
                cancionDelista.genero = await AplicacionGenero.ObtenerGeneroPorId(cancionDelista.album.idGenero);
            }
            return listaCanciones;
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

        private async void btn_Reproducir_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Cancion cancion = button.DataContext as Cancion;
            if (Reproductor.ValidarConexionCliente())
            {
                await Reproductor.Reproducir(cancion);
                SingletonReproductor.GetPaginaPrincipal().CargarInformacionAsync(cancion);
            }
            else
            {
                MessageBox.Show("No ha conexión con el cliente de Reproducción", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

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
            Button button = sender as Button;
            Cancion cancion = button.DataContext as Cancion;
            GenerarRadio(cancion);
        }
        private async void GenerarRadio(Cancion cancion)
        {
            try
            {
                List<Album> listaAlbumes = await ObtenerAlbumesAsync(cancion);
                List<Cancion> listaCancionesParaRadio = new List<Cancion>();
                foreach (var albumDelista in listaAlbumes)
                {
                    listaCancionesParaRadio.AddRange(await AplicacionCancion.ObtenerCancionesPorIdAlbumAsync(albumDelista.idAlbum));

                }
                foreach (var cancionDeLista in listaCancionesParaRadio)
                {
                    cancionDeLista.imagenPortadaCancion = await AplicacionAlbum.ObtenerImagenAlbum(cancionDeLista.portada);
                    cancionDeLista.meGusta = await AplicacionPlaylist.ValidarCancionEnMeGusta(cancionDeLista.idCancion, cuenta.idCuenta);
                }
                IniciarRadio(listaCancionesParaRadio);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private async Task<List<Album>> ObtenerAlbumesAsync(Cancion cancion)
        {
            List<Album> listaAlbumes = await AplicacionGenero.ObtenerAlbumesPorGenero(cancion.genero.idGenero);
            return listaAlbumes;
        }
        private void IniciarRadio(List<Cancion> listaCanciones)
        {
            Reproductor.ColaCanciones.Clear();
            Reproductor.AgregarListaCancionesACola(listaCanciones);
            SingletonReproductor.GetPaginaPrincipal().SiguienteCancion();
            MessageBox.Show("Se ha generado tu radio y se ha agregado a la cola de reporducción", "Acción completada", MessageBoxButton.OK);
        }
        private void Btn_AgregarTodasLasCanciones_Click(object sender, RoutedEventArgs e)
        {
            Reproductor.ColaCanciones.Clear();
            if (Reproductor.ValidarConexionCliente())
            {
                Reproductor.AgregarListaCancionesACola(listaCanciones);
                SingletonReproductor.GetPaginaPrincipal().SiguienteCancion();
            }
            else
            {
                MessageBox.Show("No ha conexión con el cliente de Reproducción", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Btn_RegresarAHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Home());
        }

        private async void btn_AñadirMegusta_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Cancion cancion = button.DataContext as Cancion;
            try
            {
                if (!await AplicacionPlaylist.ValidarCancionEnMeGusta(cancion.idCancion, cuenta.idCuenta))
                {
                    if (await AplicacionPlaylist.AgregarMeGusta(cancion.idCancion, cuenta.idCuenta))
                    {
                        cancion.meGusta = true;
                        MessageBox.Show("Canción añadida a tus Me gusta", "Realizado", MessageBoxButton.OK);
                    }
                }
                else
                {
                    MessageBox.Show("La canción seleccionada ya se encuentra en tus Me gusta", "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private async void btn_descargarCancion_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Cancion cancion = button.DataContext as Cancion;
            if (!await DescargarCancion.ValidarCancionDescargada(cancion, cuenta))
            {
                if (await DescargarCancion.Descargar(cancion, cuenta))
                {
                    MessageBox.Show(cancion.nombre + " se agregó a tu lista de descargas", "Realizado", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("No hay conexión con el servidor", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("La canción ya ha sido descargada anteriormente", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Btn_Regresar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Biblioteca());
        }

        private async void Btn_EliminarPlaylist_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Está seguro de eliminar la playlist?", "Eliminar Playlist", MessageBoxButton.OKCancel);
            if (messageBoxResult == MessageBoxResult.OK)
            {
                if (await EliminarPlaylist())
                {
                    MessageBox.Show("Playlist Eliminada con éxito", "Realizado");
                    NavigationService.Navigate(new Biblioteca());
                }

            }
        }
        private async Task<bool> EliminarPlaylist()
        {
            try
            {
                if (await AplicacionPlaylist.EliminarPlaylistById(playlist.idPlaylist))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return false;
        }

        private async void btn_eliminarDePlaylist_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Está seguro de eliminar la cancion de "+playlist.nombre+"?", "Eliminar Canción", MessageBoxButton.OKCancel);
            if (messageBoxResult == MessageBoxResult.OK)
            {
                Cancion cancion = ObtenerCancionSeleccionada();
                if (cancion !=null)
                {
                    if (await ElimnarCancionDePlaylist(cancion.idCancion))
                    {
                        MessageBox.Show("Canción eliminada con éxito", "Realizado");
                        NavigationService.Navigate(new MostrarCancionesPlaylist(playlist));
                    }
                }
                else
                {
                    MessageBox.Show("Debe seleccionar una canción de la lista", "Realizado");
                }
            }

        }
        private async Task<bool> ElimnarCancionDePlaylist(string idCancion)
        {
            try
            {
                return await AplicacionPlaylist.EliminarCancionDePlaylist(playlist.idPlaylist,idCancion);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return false;
        }
        private Cancion ObtenerCancionSeleccionada()
        {
            Cancion cancionSeleccionda = (Cancion)listView_Canciones.SelectedItem;
            return cancionSeleccionda;
        }

        private void OcultarCamposParaHome()
        {
            Btn_Regresar.Visibility = Visibility.Hidden;
            Btn_EliminarPlaylist.Visibility = Visibility.Hidden;
            Btn_EliminarCancionDePlaylist.Visibility = Visibility.Hidden;
        }
    }
}

