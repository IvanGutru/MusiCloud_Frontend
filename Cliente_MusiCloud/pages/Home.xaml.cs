using Cliente_MusiCloud.playlist.aplicacion;
using Cliente_MusiCloud.playlist.dominio;
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
    /// Lógica de interacción para Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        List<Playlist> listaPlaylistSistema;
        public Home()
        {
            InitializeComponent();
            CargarPlaylistSistemaAsync();
        }

        private async void CargarPlaylistSistemaAsync()
        {
            try
            {
                listaPlaylistSistema = await AplicacionPlaylist.ObtenerPlaylistSistema();
                foreach (var playlist in listaPlaylistSistema )
                {
                    playlist.imagenPortada = await AplicacionPlaylist.ObtenerImagenPlaylist(playlist.portada);
                }
                listViewPlaylistSistema.ItemsSource = listaPlaylistSistema;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Ocurrió un error",MessageBoxButton.OK, MessageBoxImage.Warning);
                Console.WriteLine(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CrearPlaylist());
        }
    }
}
