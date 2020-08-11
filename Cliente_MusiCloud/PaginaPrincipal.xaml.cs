
using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.artista.aplicacion;
using Cliente_MusiCloud.cancion.dominio;
using Cliente_MusiCloud.pages;
using Cliente_MusiCloud.reproductor;
using Cliente_MusiCloud.utilidades;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Cliente_MusiCloud
{
    /// <summary>
    /// Lógica de interacción para PaginaPrincipal.xaml
    /// </summary>
    public partial class PaginaPrincipal : Window
    {
        DispatcherTimer loadProgressTrackTimer;
        public PaginaPrincipal()
        {
            InitializeComponent();
            txt_UserName.Text = SingletonCuenta.GetSingletonCuenta().nombreUsuario;
            centralFrame.Navigate(new Home());
            centralFrame.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            ValidarEsCreadorContenido();
            InitializeWindow();
            ItemGeneros.Visibility = Visibility.Hidden;
        }
        public void InitializeWindow()
        {
            SingletonReproductor.SetPaginaPrincipal(this);
            loadProgressTrackTimer = new DispatcherTimer();
            loadProgressTrackTimer.Tick += new EventHandler(PrintProgress);
            loadProgressTrackTimer.Interval = new TimeSpan(0, 0, 0, 1);
 
        }
        public async void CargarInformacionAsync(Cancion cancion)
        {
            txt_Nombre.Text = cancion.nombre;
            PortadaCancion.Source = await AplicacionAlbum.ObtenerImagenAlbum(cancion.portada);
            ContinuarReproduccion();
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
            centralFrame.Navigate(new ModificarCuenta());
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
                    centralFrame.Navigate(new MostrarAlbumes());
                    break;
                case "ItemArtista":
                    centralFrame.Navigate(new MostrarArtistas());
                    break;
                case "ItemBiblioteca":
                    break;
                case "ItemGeneros":
                    break;
                case "ItemModuloArtista":
                    centralFrame.Navigate(new GestionArtista());
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
        private void ValidarEsCreadorContenido()
        {
            bool esCreador = SingletonCuenta.GetSingletonCuenta().creadorContenido;
            if (!esCreador)
            {
                ItemModuloArtista.Visibility = Visibility.Hidden;
            }
        }

        private void btn_Reproducir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Reproductor.EstaReproduciendose())
                {
                    PararCancion();
                }
                else
                {
                    ContinuarReproduccion();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void PararCancion()
        {
            if (Reproductor.PararReproduccion())
            {
                Play_icon.Kind = (MaterialDesignThemes.Wpf.PackIconKind)Enum.Parse(typeof(MaterialDesignThemes.Wpf.PackIconKind), "Play");
                loadProgressTrackTimer.Stop();
            }
        }
        private void ContinuarReproduccion()
        {
            if (Reproductor.ComenzarReproduccion())
            {
                Play_icon.Kind = (MaterialDesignThemes.Wpf.PackIconKind)Enum.Parse(typeof(MaterialDesignThemes.Wpf.PackIconKind), "Pause");
                loadProgressTrackTimer.Start();
            }
        }

        private void PrintProgress(object sender, EventArgs e)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(Reproductor.ObtenerTotalSegundosCancion());
            txt_duracionFinal.Text = string.Format("{0}:{1}", timeSpan.Duration().Minutes, timeSpan.Duration().Seconds);
            var time = Reproductor.ObtenerSegundosActuales();
            TimeSpan timeInitial = TimeSpan.FromSeconds(time);
            txt_DuracionInicial.Text = timeInitial.ToString(@"mm\:ss");
            ProgresoDuration.Value = Reproductor.ObtenerTiempoParaBarra();
            if (Reproductor.TerminoCancion())
            {
                PararCancion();
                SiguienteCancion();
            }
        }
        public async void SiguienteCancion()
        {
            Cancion cancion = await Reproductor.ReproducirSiguienteCancion();
            if (cancion != null)
            {
                ContinuarReproduccion();
                CargarInformacionAsync(cancion);
            }
        }
        private void barra_volumen_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Reproductor.ActualizarVolumen(barra_volumen.Value);
        }

        private void progreso_cancion_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Reproductor.ActualizarPosicionCancion(ProgresoDuration.Value);
        }

        private void btn_siguiente_Click(object sender, RoutedEventArgs e)
        {
            SiguienteCancion();
        }
    }
}
