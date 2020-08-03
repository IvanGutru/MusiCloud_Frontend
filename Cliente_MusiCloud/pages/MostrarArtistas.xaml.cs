using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.album.dominio;
using Cliente_MusiCloud.artista.aplicacion;
using Cliente_MusiCloud.artista.Dominio;
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
    /// Lógica de interacción para MostrarArtistas.xaml
    /// </summary>
    public partial class MostrarArtistas : Page
    {
        public MostrarArtistas()
        {
            InitializeComponent();
            txt_textoAlbumes.Visibility = Visibility.Hidden; 
            txt_nombreArtista.Visibility = Visibility.Hidden; 
        }



        private async void Btn_Buscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidateField())
                {
                    List<Artista> listaArtistas;
                    String nombre = txt_Nombre.Text;
                    listaArtistas= await Aplicacion.ObtenerArtistaPorNombre(nombre);
                    listViewArtistas.ItemsSource = listaArtistas;
                }
                else
                {
                    MessageBox.Show("Favor de ingresar un nombre de artista");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private async void listViewArtistas_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            List<Album> listaAlbumes;
            Artista artista = (Artista)listViewArtistas.SelectedItem;
            Console.WriteLine(artista.nombre);
            if (artista != null)
            {
                try
                {
                    listaAlbumes = await AplicacionAlbum.ObtenerAlbumesArtistaPorId(artista.idArtista);
                    listView_Albumes.ItemsSource = listaAlbumes;
                    txt_nombreArtista.Text = artista.nombre;
                    txt_textoAlbumes.Visibility = Visibility.Visible;
                    txt_nombreArtista.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }
        private void ListView_Albumes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Album album = (Album)listView_Albumes.SelectedItem;
            if (listView_Albumes.SelectedItems.Count>0)
            {
              NavigationService.Navigate(new MostrarCanciones(album));
            }
        }
    }
}
