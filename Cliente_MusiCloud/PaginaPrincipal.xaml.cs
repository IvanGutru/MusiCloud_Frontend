using Cliente_MusiCloud.pages;
using Cliente_MusiCloud.utilidades;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Cliente_MusiCloud
{
    /// <summary>
    /// Lógica de interacción para PaginaPrincipal.xaml
    /// </summary>
    public partial class PaginaPrincipal : Window
    {
        public PaginaPrincipal()
        {
            InitializeComponent();
            //txt_UserName.Text = SingletonCuenta.GetSingletonCuenta().cuenta.nombreUsuario;
            centralFrame.Navigate(new Home());
            centralFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void Button_account_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_signout_Click(object sender, RoutedEventArgs e)
        {
            SingletonCuenta.SetCuenta(null);
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "ItemHome":
                    centralFrame.Navigate(new Home());
                    break;
                case "ItemAlbum":
                    break;
                case "ItemArtista":
                    break;
                case "ItemBiblioteca":
                    break;
                case "ItemGeneros":
                    break;
                case "ItemExit":
                    Salir();
                    break;
                default:
                    break;
            }
        }

        private void Salir()
        {
            SingletonCuenta.SetCuenta(null);
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void ItemHome_Selected(object sender, RoutedEventArgs e) {
            centralFrame.Navigate(new Home());
            centralFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
        }

        private void btnPrevious_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) {
            var fadeAnimation = new DoubleAnimation();
            fadeAnimation.From = 1;
            fadeAnimation.To = 0.5;
            btnPrevious.BeginAnimation(Image.OpacityProperty, fadeAnimation);
        }

        private void btnPrevious_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e) {
            var fadeAnimation = new DoubleAnimation();
            fadeAnimation.From = 0.5;
            fadeAnimation.To = 1;
            btnPrevious.BeginAnimation(Image.OpacityProperty, fadeAnimation);
        }
    }
}
