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
    /// Lógica de interacción para MostrarCancionesDesdeArtista.xaml
    /// </summary>
    public partial class MostrarCancionesDesdeArtista : Page
    {
        Album album;
        public MostrarCancionesDesdeArtista(Album album)
        {
            InitializeComponent();
            this.album = album;
            CargarInformacionAlbum();
        }

        private void CargarInformacionAlbum()
        {
            txt_Nombre.Text = album.nombre;
            txt_Compania.Text = album.compania;
            txt_Nombre.IsEnabled = false;
            txt_Compania.IsEnabled = false;
        }

        private void CargarPortadaAlbum()
        {
            
        }

        private void Btn_Regresar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GestionArtista());
        }
    }
}
