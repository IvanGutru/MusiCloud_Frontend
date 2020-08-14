using Cliente_MusiCloud.bibliotecaPropia.aplicacion;
using Cliente_MusiCloud.bibliotecaPropia.dominio;
using Cliente_MusiCloud.cancion.dominio;
using Cliente_MusiCloud.cuenta.Dominio;
using Cliente_MusiCloud.playlist.dominio;
using Cliente_MusiCloud.reproductor;
using Cliente_MusiCloud.utilidades;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;


namespace Cliente_MusiCloud.pages
{
    /// <summary>
    /// Lógica de interacción para BibliotecaPropiaPage.xaml
    /// </summary>
    public partial class BibliotecaPropiaPage : Page
    {
        Playlist playlist;
        Cuentas cuenta = SingletonCuenta.GetSingletonCuenta();
        List<BibliotecaPropia> listaCancionesBiblioteca;
        public BibliotecaPropiaPage(Playlist playlistRecibida)
        {
            InitializeComponent();
            this.playlist = playlistRecibida;
            CargarInformacionBibliotecaPropia();
            CargarCancionesBibliotecaPropia();
        }
        private async void CargarCancionesBibliotecaPropia()
        {
            try
            {
                listaCancionesBiblioteca = await AplicacionBibliotecaPropia.ObtenerBibliotecaPropia(playlist.idPlaylist,cuenta.idCuenta);
                foreach (var cancionBiblioteca in listaCancionesBiblioteca)
                {
                    cancionBiblioteca.portadaImagenBibliotecaPropia = await AplicacionBibliotecaPropia.ObtenerImagenBibliotecaPropia(cancionBiblioteca.portada);
                }
                listView_CancionesBiblioteca.ItemsSource = listaCancionesBiblioteca;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Btn_Regresar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Biblioteca());
        }
        private void CargarInformacionBibliotecaPropia()
        {
            txt_NombreCreador.Text = cuenta.nombreUsuario;
            txt_NombrePlaylist.Text = playlist.nombre;
            txt_TipoPlaylist.Text = ObtenerTipoPlaylist();
            portadaAlbum.Source = playlist.imagenPortada;
        }
        private string ObtenerTipoPlaylist()
        {
            if (playlist.publica)
            {
                return "Publica";
            }
            return "Privada";
        }
        private void Btn_AñadirBiblioteca_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CrearBibliotecaPropia(playlist));
        }
        private async void btn_Reproducir_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            BibliotecaPropia bibliotecaPropia = button.DataContext as BibliotecaPropia;
            Cancion cancion = CrearCancion(bibliotecaPropia);
            await Reproductor.Reproducir(cancion);
            SingletonReproductor.GetPaginaPrincipal().CargarInformacionAsync(cancion);
        }


        private void btn_agregarCola_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            BibliotecaPropia bibliotecaPropia = button.DataContext as BibliotecaPropia;
            Cancion cancion = CrearCancion(bibliotecaPropia);
            Reproductor.AgregarCancionACola(cancion);
        }
        private Cancion CrearCancion(BibliotecaPropia bibliotecaPropia)
        {
            Cancion cancionBiblioteca = new Cancion
            {
                idCancion = bibliotecaPropia.idBibliotecaPropia.ToString(),
                archivo = bibliotecaPropia.archivo,
                nombre = bibliotecaPropia.nombreCancion,
                imagenPortadaCancion = bibliotecaPropia.portadaImagenBibliotecaPropia,
                duracion = bibliotecaPropia.duracion
            };
            return cancionBiblioteca;
        }
        private void btn_agregarSiguiente_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Cancion cancion = button.DataContext as Cancion;
            Reproductor.AgregarSiguienteACola(cancion);
        }


        private void Btn_AgregarTodasLasCanciones_Click(object sender, RoutedEventArgs e)
        {
            Reproductor.ColaCanciones.Clear();
            List<Cancion> listaCanciones = new List<Cancion>();
            foreach (var cancionBiblioteca in listaCancionesBiblioteca)
            {
                Cancion cancion = CrearCancion(cancionBiblioteca);
                listaCanciones.Add(cancion);
            }
            Reproductor.AgregarListaCancionesACola(listaCanciones);
            SingletonReproductor.GetPaginaPrincipal().SiguienteCancion();
        }
    }
}
