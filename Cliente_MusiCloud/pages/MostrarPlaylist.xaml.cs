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
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Cliente_MusiCloud.pages
{
    /// <summary>
    /// Lógica de interacción para MostrarPlaylist.xaml
    /// </summary>
    public partial class MostrarPlaylist : Page
    {
        
        List<Playlist> listaPlaylist;
        List<Cancion> listaCanciones;
        List<PlaylistCanciones> listaPlaylistCanciones;
        Cuentas cuenta = SingletonCuenta.GetSingletonCuenta();
        Playlist playlist;
        public MostrarPlaylist()
        {
            InitializeComponent();
            this.listaCanciones = new List<Cancion>();
            CargarCancionesInicioAsync();
        }

        private void Btn_Buscar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampoVacio())
            {
                string nombrePlaylist = txt_NombrePlaylist.Text;
                CargarPlaylistPorNombre(nombrePlaylist);
            }
            else
            {
                CargarCancionesInicioAsync();
            }
        }
        private async void CargarCancionesInicioAsync()
        {
            try
            {
                listaPlaylist = await AplicacionPlaylist.ObtenerPlaylistInicio();
                foreach (var playlist in listaPlaylist)
                {
                    playlist.imagenPortada = await AplicacionPlaylist.ObtenerImagenPlaylist(playlist.portada);
                    playlist.fechaPublicacion = playlist.fechaCreacion.ToShortDateString();
                }
                listViewPlaylist.ItemsSource = listaPlaylist.OrderBy(playlist => playlist.nombre);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private async void CargarPlaylistPorNombre(string nombre)
        {
            try
            {
                listaPlaylist = await AplicacionPlaylist.ObtenerPlaylistPorNombre(nombre);
                foreach (var playlist in listaPlaylist)
                {
                    playlist.imagenPortada = await AplicacionPlaylist.ObtenerImagenPlaylist(playlist.portada);
                    playlist.fechaPublicacion = playlist.fechaCreacion.ToShortDateString();
                }
                listViewPlaylist.ItemsSource = listaPlaylist;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private bool ValidarCampoVacio()
        {
            if (String.IsNullOrEmpty(txt_NombrePlaylist.Text))
            {
                return false;
            }
            return true;
        }
        private void listViewPlaylist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            playlist = (Playlist)listViewPlaylist.SelectedItem;
            if (listViewPlaylist.SelectedItems.Count>0)
            {
                CargarCancionesPlaylistAsync(playlist);
            }
        }
      
        private async void CargarCancionesPlaylistAsync(Playlist playlist)
        {
            try
            {
                listaCanciones.Clear();
                listaCanciones = await ObtenerCancionesPlaylistAsync(playlist);
                listaCanciones = await ObtenerCancionesAlbumGeneroAsync(listaCanciones);
                listView_Canciones.ItemsSource = null;
                listView_Canciones.ItemsSource = listaCanciones;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private async Task<List<Cancion>> ObtenerCancionesPlaylistAsync(Playlist playlist)
        {
            listaPlaylistCanciones = await ObtenerPlaylistCanciones(playlist);
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
        private async Task<List<PlaylistCanciones>> ObtenerPlaylistCanciones(Playlist playlist)
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
            VentanaFlotante floating = new VentanaFlotante(new AgregarCancionPlaylist(cancion));
            floating.ShowDialog();
        }

        private void btn_generarRadio_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Cancion cancion = button.DataContext as Cancion;
            GenerarRadio(cancion);
        }
        private async void GenerarRadio(Cancion cancion)
        {
            List<Album> listaAlbumes;
            try
            {
                List<Cancion> listaCancionesParaRadio = new List<Cancion>();
                listaAlbumes = await AplicacionGenero.ObtenerAlbumesPorGenero(cancion.genero.idGenero);

                foreach (var albumDelista in listaAlbumes)
                {
                    listaCancionesParaRadio.AddRange(await AplicacionCancion.ObtenerCancionesPorIdAlbumAsync(albumDelista.idAlbum));

                }
                foreach (var cancionDeLista in listaCancionesParaRadio)
                {
                    cancionDeLista.imagenPortadaCancion = await AplicacionAlbum.ObtenerImagenAlbum(cancionDeLista.portada);
                    //cancionDeLista.genero = album.genero;
                }
                Reproductor.ColaCanciones.Clear();
                Reproductor.AgregarListaCancionesACola(listaCancionesParaRadio);
                SingletonReproductor.GetPaginaPrincipal().SiguienteCancion();
                MessageBox.Show("Se ha generado tu radio y se ha agregado a la cola de reporducción", "Acción completada", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
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

     
    }
}
