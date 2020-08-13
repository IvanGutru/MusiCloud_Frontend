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

namespace Cliente_MusiCloud
{
    /// <summary>
    /// Lógica de interacción para VentanaFlotante.xaml
    /// </summary>
    public partial class VentanaFlotante : Window
    {
        public VentanaFlotante(Page page)
        {
            InitializeComponent();
            FrameCentral.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            FrameCentral.Navigate(page);
            
        }

        private void Btn_Salir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
