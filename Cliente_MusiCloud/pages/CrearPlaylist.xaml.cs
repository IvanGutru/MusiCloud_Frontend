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
    /// Lógica de interacción para CrearPlaylist.xaml
    /// </summary>
    public partial class CrearPlaylist : Page
    {
        public CrearPlaylist()
        {
            InitializeComponent();
        }

        private void Btn_Regresar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Home());
        }

    
        private void subirPortada_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GuardarPlaylist_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
