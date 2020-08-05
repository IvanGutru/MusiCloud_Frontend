using Cliente_MusiCloud.playlist.aplicacion;
using Cliente_MusiCloud.playlist.dominio;
using Cliente_MusiCloud.utilidades;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        const int PLAYLISTTIPOUSUARIO = 2;
        String pathAbsolutoImagen;
        String idCuenta = SingletonCuenta.GetSingletonCuenta().cuenta.idCuenta;
        Playlist playlist = new Playlist();
        public CrearPlaylist()
        {
            InitializeComponent();
        }

        private async void GuardarPlaylist_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarNombrePlaylist())
            {
                if (ValidarSeleccionTipoPlaylist())
                {
                    playlist = CrearPlaylistIngresada();
                    try
                    {
                        bool respuesta = await AplicacionPlaylist.CrearPlaylist(playlist);
                        if (respuesta)
                        {
                            MessageBox.Show("Playlist Registrada con éxito","Playlist Registrada",MessageBoxButton.OK,MessageBoxImage.Information);
                            NavigationService.Navigate(new Home());
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message,"Error", MessageBoxButton.OK,MessageBoxImage.Warning);
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            else
            {
                MessageBox.Show("Por favor ingrese un nombre para la playlist");
            }
        }

        private Playlist CrearPlaylistIngresada()
        {
            Playlist playlist = new Playlist
            {
                nombre = txt_NombrePlaylist.Text,
                publica = EsPlaylistPublica(),
                fechaCreacion = DateTime.Now,
                portada = ObtenerPortada(),
                idCuenta = idCuenta,
                idTipoPlaylist = PLAYLISTTIPOUSUARIO
               
            };
            return playlist;
        }
        private String ObtenerPortada()
        {
            string portadaPlaylist;
            if (pathAbsolutoImagen !=null)
            { 
              return portadaPlaylist = CodificacionImagenes.CodificarBase64(pathAbsolutoImagen);     
            }
                return portadaPlaylist ="";
        }
        private bool ValidarNombrePlaylist()
        {
            if (String.IsNullOrEmpty(txt_NombrePlaylist.Text))
                return false;
            return true;
        }
        public bool ValidarSeleccionTipoPlaylist()
        {
            if (rdb_privada.IsChecked == false && rdb_publica.IsChecked == false )
            {
                MessageBox.Show("Por favor selecciona el tipo de playlist (Pública o Privada)","Advertencia",MessageBoxButton.OK,MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        private bool EsPlaylistPublica()
        {
            return rdb_publica.IsChecked.Value;
        }
        private void subirPortada_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Formato de archivos ¨(*.jpg, *jpeg, *.png)|*.jpg; *.jpeg; *.png";
            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    string imagen = openFileDialog.FileName;
                    pathAbsolutoImagen = imagen;
                    PortadaCancion.Source = new BitmapImage(new Uri(imagen, UriKind.Absolute));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void Btn_Regresar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Home());
        }
    }
}
