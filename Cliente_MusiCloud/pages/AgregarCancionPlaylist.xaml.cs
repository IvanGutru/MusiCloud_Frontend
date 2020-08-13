using Cliente_MusiCloud.cancion.dominio;
using Cliente_MusiCloud.cuenta.Dominio;
using Cliente_MusiCloud.playlist.aplicacion;
using Cliente_MusiCloud.playlist.dominio;
using Cliente_MusiCloud.playlistCanciones.aplicacion;
using Cliente_MusiCloud.utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Lógica de interacción para AgregarCancionPlaylist.xaml
    /// </summary>
    public partial class AgregarCancionPlaylist : Page
    {
        Cuentas cuenta = SingletonCuenta.GetSingletonCuenta();
        List<Playlist> listaPlaylistUsuario;
        Cancion cancion;
        public AgregarCancionPlaylist(Cancion cancionRecibida)
        {
            InitializeComponent();
            CargarPlaylist();
            this.cancion = cancionRecibida;
        }

        private async void CargarPlaylist()
        {
            try
            {
                listaPlaylistUsuario = await AplicacionPlaylist.ObtenerPlaylistTipoUsuario(cuenta.idCuenta);
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

        private async void listViewMisPlaylist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Playlist playlistSeleccionada = (Playlist)listViewMisPlaylist.SelectedItem;
            if (await GuardarCancionEnPlaylistAsync(playlistSeleccionada.idPlaylist,cancion.idCancion))
            {
                MessageBox.Show("Se guardó la canción en la playlist: "+playlistSeleccionada.nombre, "Realizado", MessageBoxButton.OK);
                var parent = Application.Current.Windows.OfType<VentanaFlotante>().FirstOrDefault();
                if (parent != null)
                {
                    parent.Close();
                }
            }
        }

        private async Task<bool> GuardarCancionEnPlaylistAsync(int idPlaylist,string idCancion)
        {
            bool respuesta = false;
            try
            {
                respuesta = await AplicacionPlaylistCanciones.AgregarCancionAPlaylist(idPlaylist,idCancion);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return respuesta;
        }
    }
}
