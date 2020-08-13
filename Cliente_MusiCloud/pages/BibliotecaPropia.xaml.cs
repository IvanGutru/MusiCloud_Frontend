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
    /// Lógica de interacción para BibliotecaPropia.xaml
    /// </summary>
    public partial class BibliotecaPropia : Page
    {
        public BibliotecaPropia(Playlist playlistRecibida)
        {
            InitializeComponent();
        }

        private void Btn_Regresar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Biblioteca());
        }
    }
}
