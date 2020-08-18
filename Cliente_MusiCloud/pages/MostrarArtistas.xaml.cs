using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.album.dominio;
using Cliente_MusiCloud.artista.aplicacion;
using Cliente_MusiCloud.artista.Dominio;
using Cliente_MusiCloud.genero.aplicacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Cliente_MusiCloud.pages
{
    /// <summary>
    /// Lógica de interacción para MostrarArtistas.xaml
    /// </summary>
    public partial class MostrarArtistas : Page
    {
        List<Artista> listaArtistas;
        List<Album> listaAlbumes;
        public MostrarArtistas()
        {
            InitializeComponent();
            txt_textoAlbumes.Visibility = Visibility.Hidden;
            txt_nombreArtista.Visibility = Visibility.Hidden;
            CargarArtistasInicio();
        }



        private void Btn_Buscar_Click(object sender, RoutedEventArgs e)
        {

            if (ValidateField())
            {
                String nombre = txt_Nombre.Text;
                CargarArtistas(nombre);
            }
            else
            {
                CargarArtistasInicio();
            }


        }
        private async void CargarArtistas(string nombre)
        {
            try
            {
                listaArtistas = await Aplicacion.ObtenerArtistaPorNombre(nombre);
                foreach (var artistalista in listaArtistas)
                {
                    artistalista.imagenPortadaArtista = await Aplicacion.ObtenerImagenArtista(artistalista.portada);
                    artistalista.genero = await AplicacionGenero.ObtenerGeneroPorId(artistalista.idGenero);
                }
                listViewArtistas.ItemsSource = listaArtistas;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private async void CargarArtistasInicio()
        {
            try
            {
                listaArtistas = await Aplicacion.ObtenerArtistaHome();
                foreach (var artistasHome in listaArtistas)
                {
                    artistasHome.imagenPortadaArtista = await Aplicacion.ObtenerImagenArtista(artistasHome.portada);
                }
                listViewArtistas.ItemsSource = listaArtistas.OrderBy(artista => artista.nombre);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool ValidateField()
        {
            if (String.IsNullOrEmpty(txt_Nombre.Text))
            {
                return false;
            }
            return true;
        }

        private  void listViewArtistas_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {

            Artista artistaSeleccionado = (Artista)listViewArtistas.SelectedItem;
            CargarAlbumesArtista(artistaSeleccionado.idArtista);
            ConfigurarCamposAlbum(artistaSeleccionado.nombre);
           

        }
        private async void CargarAlbumesArtista(string idArtista)
        {
            try
            {
                listaAlbumes = await AplicacionAlbum.ObtenerAlbumesArtistaPorId(idArtista);
                foreach (var albumDeLista in listaAlbumes)
                {
                    albumDeLista.imagenPortadaAlbum = await AplicacionAlbum.ObtenerImagenAlbum(albumDeLista.portada);
                    albumDeLista.genero = await AplicacionGenero.ObtenerGeneroPorId(albumDeLista.idGenero);
                }
                listView_Albumes.ItemsSource = listaAlbumes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void ConfigurarCamposAlbum(string nombre)
        {
            txt_nombreArtista.Text = nombre;
            txt_textoAlbumes.Visibility = Visibility.Visible;
            txt_nombreArtista.Visibility = Visibility.Visible;
        }
        private void ListView_Albumes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Album album = (Album)listView_Albumes.SelectedItem;
            Artista artistaSelec = (Artista)listViewArtistas.SelectedItem;
            if (listView_Albumes.SelectedItems.Count > 0)
            {
                NavigationService.Navigate(new MostrarCanciones(album,artistaSelec));
            }
        }
    }
}
