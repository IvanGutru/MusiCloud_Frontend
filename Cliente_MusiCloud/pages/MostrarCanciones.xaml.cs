using Cliente_MusiCloud.album.dominio;
using Cliente_MusiCloud.cancion.aplicacion;
using Cliente_MusiCloud.cancion.dominio;
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
    /// Lógica de interacción para MostrarCanciones.xaml
    /// </summary>
    public partial class MostrarCanciones : Page
    {
        Album album;
        List<Cancion> listaCanciones;
        public MostrarCanciones(Album albumLista)
        {
            album = albumLista;
            InitializeComponent();
            txt_NombreAlbum.Text = album.nombre;
            txt_NombreCompania.Text = album.compania;
            CargarCanciones();

        }

        private async void CargarCanciones()
        {
            if (album != null)
            {
                try
                {
                    listaCanciones = await AplicacionCancion.ObtenerCancionesPorIdAlbumAsync(album.idAlbum);
                    listView_Canciones.ItemsSource = listaCanciones;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Btn_Regresar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MostrarArtistas());
        }
    }
}
