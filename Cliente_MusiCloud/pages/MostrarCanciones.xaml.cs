using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.album.dominio;
using Cliente_MusiCloud.artista.Dominio;
using Cliente_MusiCloud.cancion.aplicacion;
using Cliente_MusiCloud.cancion.dominio;
using Cliente_MusiCloud.cuenta.Dominio;
using Cliente_MusiCloud.genero.aplicacion;
using Cliente_MusiCloud.playlist.aplicacion;
using Cliente_MusiCloud.reproductor;
using Cliente_MusiCloud.utilidades;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Cliente_MusiCloud.pages
{
    /// <summary>
    /// Lógica de interacción para MostrarCanciones.xaml
    /// </summary>
    public partial class MostrarCanciones : Page
    {
        Album album;
        Artista artista;
        List<Cancion> listaCanciones;
        Cuentas cuenta = SingletonCuenta.GetSingletonCuenta();
        public MostrarCanciones(Album album, Artista artista)
        {
            this.album = album;
            this.artista = artista;
            InitializeComponent();
            CargarCanciones();
            CargarCamposAlbum();
            Btn_RegresarAHome.Visibility = Visibility.Hidden;

        }
        public MostrarCanciones(Album album)
        {
            InitializeComponent();
            this.album = album;
            CargarCanciones();
            CargarCamposDesdeHome();
            Btn_Regresar.Visibility = Visibility.Hidden;
        }
       
        private async void CargarCanciones()
        {
            try
            {
                listaCanciones = await AplicacionCancion.ObtenerCancionesPorIdAlbumAsync(album.idAlbum);
                foreach (var cancionDeLista in listaCanciones)
                {
                    cancionDeLista.imagenPortadaCancion = await AplicacionAlbum.ObtenerImagenAlbum(cancionDeLista.portada);
                    cancionDeLista.genero = album.genero;
                    cancionDeLista.meGusta = await AplicacionPlaylist.ValidarCancionEnMeGusta(cancionDeLista.idCancion,cuenta.idCuenta);
                }
                listView_Canciones.ItemsSource = listaCanciones;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        private void CargarCamposAlbum()
        {
            txt_NombreAlbum.Text = album.nombre;
            txt_NombreCompania.Text = album.compania;
            txt_NombreArtista.Text = artista.nombre;
            portadaAlbum.Source = album.imagenPortadaAlbum;
        }
        private void CargarCamposDesdeHome()
        {
            txt_NombreAlbum.Text = album.nombre;
            txt_NombreCompania.Text = album.compania;
            txt_NombreArtista.Text = album.artista.nombre;
            portadaAlbum.Source = album.imagenPortadaAlbum;
        }
        private void Btn_Regresar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MostrarArtistas());
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
                    cancionDeLista.meGusta = await AplicacionPlaylist.ValidarCancionEnMeGusta(cancionDeLista.idCancion,cuenta.idCuenta);
                }
                Reproductor.ColaCanciones.Clear();
                Reproductor.AgregarListaCancionesACola(listaCancionesParaRadio);
                SingletonReproductor.GetPaginaPrincipal().SiguienteCancion();
                MessageBox.Show("Se ha generado tu radio y se ha agregado a la cola de reproducción", "Acción completada", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Btn_AgregarTodasLasCanciones_Click(object sender, RoutedEventArgs e)
        {
            Reproductor.ColaCanciones.Clear();
            Reproductor.AgregarListaCancionesACola(listaCanciones);
            SingletonReproductor.GetPaginaPrincipal().SiguienteCancion();
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

        private void Btn_RegresarAHome_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Home());
        }
    }
}
