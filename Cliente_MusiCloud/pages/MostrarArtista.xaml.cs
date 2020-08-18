using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.album.dominio;
using Cliente_MusiCloud.artista.Dominio;
using Cliente_MusiCloud.genero.aplicacion;
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
    /// Lógica de interacción para MostrarArtista.xaml
    /// </summary>
    public partial class MostrarArtista : Page
    {
        List<Album> listaAlbumes;
        Artista artista;
        public MostrarArtista(Artista artista)
        {
            InitializeComponent();
            this.artista = artista;
            CargarAlbumesArtista();
            CargarInformacionArtista();
        }

        private async void CargarAlbumesArtista()
        {

            try
            {
                listaAlbumes = await AplicacionAlbum.ObtenerAlbumesArtistaPorId(artista.idArtista);
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

        private void CargarInformacionArtista()
        {
            txt_Nombre.Text = artista.nombre;
            txt_Descripcion.Text = artista.descripcion;
            portadaArtista.Source = artista.imagenPortadaArtista;

        }
        private void listView_Albumes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Album album = (Album)listView_Albumes.SelectedItem;
            if (listView_Albumes.SelectedItems.Count > 0)
            {
                NavigationService.Navigate(new MostrarCancionesDesdeArtista(artista,album));
            }
        }

        private void Btn_Regresar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Home());
        }
    }
}
