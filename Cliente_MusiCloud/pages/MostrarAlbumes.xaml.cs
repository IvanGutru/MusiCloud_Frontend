using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.album.dominio;
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
    /// Lógica de interacción para MostrarAlbumes.xaml
    /// </summary>
    public partial class MostrarAlbumes : Page
    {
        public MostrarAlbumes()
        {
            InitializeComponent();
        }

        private async void Btn_Buscar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampoVacio())
            {
                try
                {
                    string nombreAlbum = txt_NombreAlbum.Text;
                    List<Album> listaAlbumes = await AplicacionAlbum.ObtenerAlbumPorNombre(nombreAlbum);
                    listViewAlbumes.ItemsSource = listaAlbumes;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
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
            Album album = (Album)listViewAlbumes.SelectedItem;
            if (listViewAlbumes.SelectedItems.Count>0)
            {
                NavigationService.Navigate(new MostrarCanciones(album.idAlbum));
            }
        }
    }
}
