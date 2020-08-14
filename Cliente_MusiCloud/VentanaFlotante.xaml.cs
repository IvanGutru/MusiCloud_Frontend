using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

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
