using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.album.dominio;
using Cliente_MusiCloud.artista.aplicacion;
using Cliente_MusiCloud.artista.Dominio;
using Cliente_MusiCloud.genero.aplicacion;
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
    /// Lógica de interacción para GestionArtista.xaml
    /// </summary>
    public partial class GestionArtista : Page
    {
        Artista artistaSingleton = SingletonArtista.GetArtista();
        List<Album> listaAlbumes;
        public GestionArtista()
        {
            InitializeComponent();
            CargarImagenArtistaAsync();
            CargarInformacionArtista();
            CargarAlbumesArtista();
        }

        private async void CargarAlbumesArtista()
        {
  
            try
            {
                listaAlbumes = await AplicacionAlbum.ObtenerAlbumesArtistaPorId(artistaSingleton.idArtista);
                foreach (var albumEnLista in listaAlbumes)
                {
                    albumEnLista.imagenPortadaAlbum = await AplicacionAlbum.ObtenerImagenAlbum(albumEnLista.portada);
                    albumEnLista.genero = await AplicacionGenero.ObtenerGeneroPorId(albumEnLista.idGenero);
                }
                listView_Albumes.ItemsSource = listaAlbumes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private async void CargarImagenArtistaAsync()
        {
            try
            {
                BitmapImage imagen = await Aplicacion.ObtenerImagenArtista(artistaSingleton.portada);
                portadaArtista.Source = imagen;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void CargarInformacionArtista()
        {
            txt_Nombre.Text = artistaSingleton.nombre;
            txt_Descripcion.Text = artistaSingleton.descripcion;
         
        }

        private void listView_Albumes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Album album = (Album)listView_Albumes.SelectedItem;
            if (listView_Albumes.SelectedItems.Count > 0)
            {
                NavigationService.Navigate(new MostrarCancionesDesdeArtista(album));
            }
        }

        private void btn_SubirAlbum_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CrearAlbum());
        }
    }
}
