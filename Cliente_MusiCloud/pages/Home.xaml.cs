﻿using Cliente_MusiCloud.album.aplicacion;
using Cliente_MusiCloud.album.dominio;
using Cliente_MusiCloud.artista.aplicacion;
using Cliente_MusiCloud.artista.Dominio;
using Cliente_MusiCloud.playlist.aplicacion;
using Cliente_MusiCloud.playlist.dominio;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Cliente_MusiCloud.pages
{
    /// <summary>
    /// Lógica de interacción para Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        List<Playlist> listaPlaylistSistema;
        List<Artista> listaArtistasHome;
        List<Album> listaAlbumesHome;
        public Home()
        {
            InitializeComponent();
            CargarPlaylistSistemaAsync();
            CargarArtistas();
            CargarAlbumes();
        }

        private async void CargarPlaylistSistemaAsync()
        {
            try
            {
                listaPlaylistSistema = await AplicacionPlaylist.ObtenerPlaylistSistema();
                foreach (var playlist in listaPlaylistSistema )
                {
                    playlist.imagenPortada = await AplicacionPlaylist.ObtenerImagenPlaylist(playlist.portada);
                }
                listViewPlaylistSistema.ItemsSource = listaPlaylistSistema;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Ocurrió un error",MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private async void CargarArtistas()
        {
            try
            {
                listaArtistasHome = await Aplicacion.ObtenerArtistaHome();
                foreach (var artistasHome in listaArtistasHome)
                {
                    artistasHome.imagenPortadaArtista = await Aplicacion.ObtenerImagenArtista(artistasHome.portada);
                }
                listViewArtistas.ItemsSource = listaArtistasHome;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private async void CargarAlbumes()
        {
            try
            {

                listaAlbumesHome = await AplicacionAlbum.ObtenerAlbumHome();
                foreach (var albumHome in listaAlbumesHome)
                {
                    albumHome.imagenPortadaAlbum = await Aplicacion.ObtenerImagenArtista(albumHome.portada);
                }
                listViewAlbum.ItemsSource = listaAlbumesHome;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CrearPlaylist());
        }
    }
}
