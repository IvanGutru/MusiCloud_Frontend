using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.album.dominio;
using Cliente_MusiCloud.artista.aplicacion;
using Cliente_MusiCloud.cancion.aplicacion;
using Cliente_MusiCloud.cancion.dominio;
using Cliente_MusiCloud.cuenta.Dominio;
using Cliente_MusiCloud.descargar;
using Cliente_MusiCloud.genero.aplicacion;
using Cliente_MusiCloud.playlist.aplicacion;
using Cliente_MusiCloud.reproductor;
using Cliente_MusiCloud.utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cliente_MusiCloud.pages
{
    /// <summary>
    /// Lógica de interacción para MostrarAlbumes.xaml
    /// </summary>
    public partial class MostrarAlbumes : Page
    {
        List<Cancion> listaCanciones;
        List<Album> listaAlbumes;
        Album album;
        Cuentas cuenta = SingletonCuenta.GetSingletonCuenta();

        public MostrarAlbumes()
        {
            InitializeComponent();
            CargarAlbumesInicio();
        }

        private void Btn_Buscar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampoVacio())
            {
                string nombreAlbum = txt_NombreAlbum.Text;
                CargarAlbumesPorNombre(nombreAlbum);
            }
            else
            {
                CargarAlbumesInicio();
            }
   
        }
        private async void CargarAlbumesPorNombre(string nombre)
        {
            try
            {
                listaAlbumes = await AplicacionAlbum.ObtenerAlbumPorNombre(nombre);
                foreach (var albumes in listaAlbumes)
                {
                    albumes.imagenPortadaAlbum = await AplicacionAlbum.ObtenerImagenAlbum(albumes.portada);
                    albumes.fechalanzamiento = albumes.fechaRegistro.ToShortDateString();
                    albumes.genero = await AplicacionGenero.ObtenerGeneroPorId(albumes.idGenero);

                }
                listViewAlbumes.ItemsSource = listaAlbumes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private async void CargarAlbumesInicio()
        {
            try
            {
                listaAlbumes = await AplicacionAlbum.ObtenerAlbumHome();
                foreach (var albumHome in listaAlbumes)
                {
                    albumHome.imagenPortadaAlbum = await Aplicacion.ObtenerImagenArtista(albumHome.portada);
                    albumHome.fechalanzamiento = albumHome.fechaRegistro.ToShortDateString();
                    albumHome.genero = await AplicacionGenero.ObtenerGeneroPorId(albumHome.idGenero);
                }
                listViewAlbumes.ItemsSource = listaAlbumes.OrderBy(album => album.nombre);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
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
            album = (Album)listViewAlbumes.SelectedItem;
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
                    cancion.genero = albumRecibido.genero;
                    cancion.meGusta = await AplicacionPlaylist.ValidarCancionEnMeGusta(cancion.idCancion,cuenta.idCuenta);
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
                    cancionDeLista.genero = album.genero;
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
