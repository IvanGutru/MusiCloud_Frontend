﻿using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.album.dominio;
using Cliente_MusiCloud.cancion.aplicacion;
using Cliente_MusiCloud.cancion.dominio;
using Cliente_MusiCloud.reproductor;
using Cliente_MusiCloud.utilidades;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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
            CargarPortadaAlbum();
        }

        private void CargarInformacionAlbum()
        {
            txt_Nombre.Text = album.nombre;
            txt_Compania.Text = album.compania;
            txt_Nombre.IsEnabled = false;
            txt_Compania.IsEnabled = false;
        }

        private async void CargarPortadaAlbum()
        {
            try
            {
                BitmapImage imagen = await AplicacionAlbum.ObtenerImagenAlbum(album.portada);
                portadaAlbum.Source = imagen;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private async void CargarCanciones()
        {
            if (album != null)
            {
                try
                {
                    listaCanciones = await AplicacionCancion.ObtenerCancionesPorIdAlbumAsync(album.idAlbum);
                    listView_Canciones.ItemsSource = listaCanciones;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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
            SingletonReproductor.GetPaginaPrincipal().CargarInformacion(cancion);
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

        }

        private void btn_generarRadio_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
