using Cliente_MusiCloud.cuenta.Dominio;
using Cliente_MusiCloud.playlist.aplicacion;
using Cliente_MusiCloud.playlist.dominio;
using Cliente_MusiCloud.reproductor;
using Cliente_MusiCloud.utilidades;
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
    /// Lógica de interacción para Biblioteca.xaml
    /// </summary>
    public partial class Biblioteca : Page
    {
        Cuentas cuenta = SingletonCuenta.GetSingletonCuenta();
        List<Playlist> listaPlaylistUsuario; 
        public Biblioteca()
        {
            InitializeComponent();
            CargarPlaylistUsuario();
            CargarColaReproduccion();
        }

        private async void CargarPlaylistUsuario()
        {
            try
            {
                listaPlaylistUsuario = await AplicacionPlaylist.ObtenerPlaylistDeUsuario(cuenta.idCuenta);
                foreach (var playlistDelista in listaPlaylistUsuario)
                {
                    playlistDelista.imagenPortada = await AplicacionPlaylist.ObtenerImagenPlaylist(playlistDelista.portada);
                }
                listViewMisPlaylist.ItemsSource = listaPlaylistUsuario;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void CargarColaReproduccion()
        {
            listViewColaReproduccion.ItemsSource = Reproductor.ColaCanciones;
        }
    }
}
