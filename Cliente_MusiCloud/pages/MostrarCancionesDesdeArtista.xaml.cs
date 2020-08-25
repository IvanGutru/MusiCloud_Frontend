using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.album.dominio;
using Cliente_MusiCloud.artista.Dominio;
using Cliente_MusiCloud.cancion.aplicacion;
using Cliente_MusiCloud.cancion.dominio;
using Cliente_MusiCloud.cancionDescarga;
using Cliente_MusiCloud.cuenta.Dominio;
using Cliente_MusiCloud.descargar;
using Cliente_MusiCloud.genero.aplicacion;
using Cliente_MusiCloud.playlist.aplicacion;
using Cliente_MusiCloud.reproductor;
using Cliente_MusiCloud.utilidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Cliente_MusiCloud.pages
{
    /// <summary>
    /// Lógica de interacción para MostrarCancionesDesdeArtista.xaml
    /// </summary>
    public partial class MostrarCancionesDesdeArtista : Page
    {
        Album album;
        Artista artista;
        List<Cancion> listaCanciones;
        Cuentas cuenta = SingletonCuenta.GetSingletonCuenta();
        public MostrarCancionesDesdeArtista(Album album)
        {
            InitializeComponent();
            this.album = album;
            CargarInformacionAlbum();
            CargarCanciones();
            Btn_RegresarArtista.Visibility = Visibility.Hidden;

        }
        public MostrarCancionesDesdeArtista(Artista artista, Album album)
        {
            InitializeComponent();
            this.album = album;
            this.artista = artista;
            CargarCanciones();
            CargarInformacionAlbumArtista();
            Btn_Regresar.Visibility = Visibility.Hidden;
        }

        private void CargarInformacionAlbum()
        {
            txt_NombreAlbum.Text = album.nombre;
            txt_NombreCompania.Text = album.compania;
            txt_NombreArtista.Text = SingletonArtista.GetArtista().nombre;
            portadaAlbum.Source = album.imagenPortadaAlbum;
        }

        private void CargarInformacionAlbumArtista()
        {
            txt_NombreAlbum.Text = album.nombre;
            txt_NombreCompania.Text = album.compania;
            txt_NombreArtista.Text = artista.nombre;
            portadaAlbum.Source = album.imagenPortadaAlbum;
        }



        private async void CargarCanciones()
        {
            if (album != null)
            {
                try
                {
                    listaCanciones = await AplicacionCancion.ObtenerCancionesPorIdAlbumAsync(album.idAlbum);
                    foreach (var cancionDeLista in listaCanciones)
                    {
                        cancionDeLista.imagenPortadaCancion = await AplicacionAlbum.ObtenerImagenAlbum(cancionDeLista.portada);
                        cancionDeLista.genero = album.genero;
                        cancionDeLista.album = album;
                    }
                    listView_Canciones.ItemsSource = listaCanciones;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        private void Btn_Regresar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GestionArtista());
        }

        private async void btn_Reproducir_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Cancion cancion = button.DataContext as Cancion;
            if (Reproductor.ValidarConexionCliente())
            {
                if(await Reproductor.Reproducir(cancion))
                {
                    SingletonReproductor.GetPaginaPrincipal().CargarInformacionAsync(cancion);
                }               
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
                    cancionDeLista.meGusta = await AplicacionPlaylist.ValidarCancionEnMeGusta(cancionDeLista.idCancion, cuenta.idCuenta);
                }
                Reproductor.ColaCanciones.Clear();
                Reproductor.AgregarListaCancionesACola(listaCancionesParaRadio);
                SingletonReproductor.GetPaginaPrincipal().SiguienteCancion();
                MessageBox.Show("Se ha generado tu radio y se ha agregado a la cola de reporducción", "Acción completada", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void Btn_RegresarArtista_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MostrarArtista(artista));
        }
    }
}
