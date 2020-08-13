﻿using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.album.dominio;
using Cliente_MusiCloud.cancion.aplicacion;
using Cliente_MusiCloud.cancion.dominio;
using Cliente_MusiCloud.genero.aplicacion;
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
    /// Lógica de interacción para MostrarCancionesDesdeArtista.xaml
    /// </summary>
    public partial class MostrarCancionesDesdeArtista : Page
    {
        Album album;
        List<Cancion> listaCanciones;
        public MostrarCancionesDesdeArtista(Album album)
        {
            InitializeComponent();
            this.album = album;
            CargarInformacionAlbum();
            CargarCanciones();

        }

        private void CargarInformacionAlbum()
        {
            txt_NombreAlbum.Text = album.nombre;
            txt_NombreCompania.Text = album.compania;
            txt_NombreArtista.Text = SingletonArtista.GetArtista().nombre;
            portadaAlbum.Source = album.imagenPortadaAlbum;
        }

   
        private async void CargarCanciones()
        {
            if (album != null)
            {
                try
                {
                    listaCanciones = await AplicacionCancion.ObtenerCancionesPorIdAlbumAsync(album.idAlbum);
                    foreach (var cancionDeLista in listaCanciones)
                    {
                        cancionDeLista.imagenPortadaCancion = await AplicacionAlbum.ObtenerImagenAlbum(cancionDeLista.portada);
                        cancionDeLista.genero = await AplicacionGenero.ObtenerGeneroPorId(album.idGenero);
                        cancionDeLista.album = album;
                    }
                    listView_Canciones.ItemsSource = listaCanciones;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        private void Btn_Regresar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GestionArtista());
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
            VentanaFlotante ventanaflotante = new VentanaFlotante(new AgregarCancionPlaylist(cancion));
            ventanaflotante.ShowDialog();
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
                listaAlbumes = await AplicacionGenero.ObtenerAlbumesPorGenero(cancion.genero.idGenero);
                foreach (var albumDelista in listaAlbumes)
                {
                   
                }
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
    }
}
