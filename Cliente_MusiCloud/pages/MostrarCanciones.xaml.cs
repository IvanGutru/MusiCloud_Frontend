using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.album.dominio;
using Cliente_MusiCloud.artista.Dominio;
using Cliente_MusiCloud.cancion.aplicacion;
using Cliente_MusiCloud.cancion.dominio;
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
        public MostrarCanciones(Album album, Artista artista)
        {
            this.album = album;
            this.artista = artista;
            InitializeComponent();
            CargarCanciones();
            CargarCamposAlbum();

        }
        private async void CargarCanciones()
        {
            try
            {
                listaCanciones = await AplicacionCancion.ObtenerCancionesPorIdAlbumAsync(album.idAlbum);
                foreach (var cancionDeLista in listaCanciones)
                {
                    cancionDeLista.imagenPortadaCancion = await AplicacionAlbum.ObtenerImagenAlbum(cancionDeLista.portada);
                }
                listView_Canciones.ItemsSource = listaCanciones;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void CargarCamposAlbum()
        {
            txt_NombreAlbum.Text = album.nombre;
            txt_NombreCompania.Text = album.compania;
            txt_NombreArtista.Text = artista.nombre;
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
