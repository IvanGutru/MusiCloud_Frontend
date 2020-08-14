using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.album.dominio;
using Cliente_MusiCloud.cancion.aplicacion;
using Cliente_MusiCloud.cancion.dominio;
using Cliente_MusiCloud.cuenta.Dominio;
using Cliente_MusiCloud.genero.aplicacion;
using Cliente_MusiCloud.playlist.aplicacion;
using Cliente_MusiCloud.reproductor;
using Cliente_MusiCloud.utilidades;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cliente_MusiCloud.pages
{
    /// <summary>
    /// Lógica de interacción para MostrarAlbumes.xaml
    /// </summary>
    public partial class MostrarAlbumes : Page
    {
        List<Cancion> listaCanciones;
        Album album;
        Cuentas cuenta = SingletonCuenta.GetSingletonCuenta();

        public MostrarAlbumes()
        {
            InitializeComponent();
        }

        private async void Btn_Buscar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampoVacio())
            {
                try
                {
                    string nombreAlbum = txt_NombreAlbum.Text;
                    List<Album> listaAlbumes = await AplicacionAlbum.ObtenerAlbumPorNombre(nombreAlbum);
                    foreach (var albumes in listaAlbumes)
                    {
                        albumes.imagenPortadaAlbum = await AplicacionAlbum.ObtenerImagenAlbum(albumes.portada);
                        albumes.fechalanzamiento = albumes.fechaRegistro.ToShortDateString();
                        albumes.genero = await AplicacionGenero.ObtenerGeneroPorId(albumes.idGenero);
                        
                    }
                    listViewAlbumes.ItemsSource = listaAlbumes;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
   
        }

        private bool ValidarCampoVacio()
        {
            if (String.IsNullOrEmpty(txt_NombreAlbum.Text))
            {
                return false;
            }
            return true;
        }
   

        private void listViewAlbumes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            album = (Album)listViewAlbumes.SelectedItem;
            if (listViewAlbumes.SelectedItems.Count>0)
            {
                CargarCancionesDeAlbum(album);
            }
        }

        private async void CargarCancionesDeAlbum(Album albumRecibido)
        {
            try
            {
                listaCanciones = await AplicacionCancion.ObtenerCancionesPorIdAlbumAsync(albumRecibido.idAlbum);
                foreach (var cancion in listaCanciones)
                {
                    cancion.imagenPortadaCancion = await AplicacionAlbum.ObtenerImagenAlbum(cancion.portada);
                    cancion.genero = albumRecibido.genero;
                    cancion.meGusta = await AplicacionPlaylist.ValidarCancionEnMeGusta(cancion.idCancion,cuenta.idCuenta);
                }
                listView_Canciones.ItemsSource = listaCanciones;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private async void btn_Reproducir_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Cancion cancion = button.DataContext as Cancion;
            await Reproductor.Reproducir(cancion);
            SingletonReproductor.GetPaginaPrincipal().CargarInformacionAsync(cancion);
        }


        private void btn_agregarCola_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Cancion cancion = button.DataContext as Cancion;
            Reproductor.AgregarCancionACola(cancion);
        }

        private void btn_agregarSiguiente_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Cancion cancion = button.DataContext as Cancion;
            Reproductor.AgregarSiguienteACola(cancion);
        }

        private void btn_agregarAPlaylist_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Cancion cancion = button.DataContext as Cancion;
            VentanaFlotante floating = new VentanaFlotante(new AgregarCancionPlaylist(cancion));
            floating.ShowDialog();
        }

        private void btn_generarRadio_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Cancion cancion = button.DataContext as Cancion;
            GenerarRadio(cancion);
        }
        private async void GenerarRadio(Cancion cancion)
        {
            List<Album> listaAlbumes;
            try
            {
                List<Cancion> listaCancionesParaRadio = new List<Cancion>();
                listaAlbumes = await AplicacionGenero.ObtenerAlbumesPorGenero(cancion.genero.idGenero);

                foreach (var albumDelista in listaAlbumes)
                {
                    listaCancionesParaRadio.AddRange(await AplicacionCancion.ObtenerCancionesPorIdAlbumAsync(albumDelista.idAlbum));

                }
                foreach (var cancionDeLista in listaCancionesParaRadio)
                {
                    cancionDeLista.imagenPortadaCancion = await AplicacionAlbum.ObtenerImagenAlbum(cancionDeLista.portada);
                    cancionDeLista.genero = album.genero;
                }
                Reproductor.ColaCanciones.Clear();
                Reproductor.AgregarListaCancionesACola(listaCancionesParaRadio);
                SingletonReproductor.GetPaginaPrincipal().SiguienteCancion();
                MessageBox.Show("Se ha generado tu radio y se ha agregado a la cola de reporducción", "Acción completada", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void Btn_AgregarTodasLasCanciones_Click(object sender, RoutedEventArgs e)
        {
            Reproductor.ColaCanciones.Clear();
            Reproductor.AgregarListaCancionesACola(listaCanciones);
            SingletonReproductor.GetPaginaPrincipal().SiguienteCancion();
        }
        private async void btn_AñadirMegusta_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Cancion cancion = button.DataContext as Cancion;
            try
            {
                if (!await AplicacionPlaylist.ValidarCancionEnMeGusta(cancion.idCancion, cuenta.idCuenta))
                {
                    if (await AplicacionPlaylist.AgregarMeGusta(cancion.idCancion, cuenta.idCuenta))
                    {
                        cancion.meGusta = true;
                        MessageBox.Show("Canción añadida a tus Me gusta", "Realizado", MessageBoxButton.OK);
                    }
                }
                else
                {
                    MessageBox.Show("La canción seleccionada ya se encuentra en tus Me gusta", "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
