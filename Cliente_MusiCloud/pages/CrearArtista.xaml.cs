﻿using Cliente_MusiCloud.artista.Dominio;
using Cliente_MusiCloud.cuenta.Dominio;
using Cliente_MusiCloud.cuentaArtista.aplicacion;
using Cliente_MusiCloud.cuentaArtista.dominio;
using Cliente_MusiCloud.genero.aplicacion;
using Cliente_MusiCloud.genero.dominio;
using Cliente_MusiCloud.utilidades;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Cliente_MusiCloud.pages
{
    /// <summary>
    /// Lógica de interacción para CrearArtista.xaml
    /// </summary>
    public partial class CrearArtista : Page
    {
        List<Genero> listaGeneros;
        String pathAbsolutoImagen;
        Artista artista;
        Cuentas cuenta = SingletonCuenta.GetSingletonCuenta();
        public CrearArtista()
        {
            InitializeComponent();
            CargarGenerosAsync();
        }

        private async void CargarGenerosAsync()
        {
            try
            {
                listaGeneros = await AplicacionGenero.ObtenerGenerosAsync();
                foreach (var lista in listaGeneros)
                {
                    CoBox_Generos.Items.Add(lista.nombre);
                }
                CoBox_Generos.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
     
        private async void GuardarArtista_Click(object sender, RoutedEventArgs e)
        {
            CuentaArtista cuentaArtista = await ObtenerCuentaArtistaAsync();
            if (cuentaArtista!=null)
            {
                try
                {
                    await GuardarNuevoValorCreadorContenidoAsync();
                    await AplicacionCuentaArtista.RegistrarCuentaArtista(cuentaArtista);
                    MessageBox.Show("Artista Registrado con éxito", "Operación éxitosa", MessageBoxButton.OK);
                    NavigationService.Navigate(new Home());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        private async Task GuardarNuevoValorCreadorContenidoAsync()
        {
                CreadorRequest creadorRequest = new CreadorRequest
                {
                    IdCuenta = cuenta.idCuenta
                };
                await Cuenta.Aplicacion.ConvertirseEnCreadorDeContenido(creadorRequest);
        }
        private bool ValidarCreadorContenidoNoRegistrado()
        {
            if (cuenta.creadorContenido)
            {
                MessageBox.Show("Ya se ha registrado como creador de contenido", "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
    
        }

        private async Task<CuentaArtista> ObtenerCuentaArtistaAsync()
        {
            CuentaArtista cuentaArtista = null;
            artista = await RegistrarArtistaAsync();
            if (artista!=null)
            {
                cuentaArtista = new CuentaArtista
                {
                    idCuenta = cuenta.idCuenta,
                    idArtista = artista.idArtista
                };
            }
            return cuentaArtista;
        }
        private async Task<Artista> RegistrarArtistaAsync()
        {
            Artista artistaRegistradoEnBD = null;
            if (ValidarCampos() && ValidarCreadorContenidoNoRegistrado())
            {
                try
                {
                    artista = await ObtenerInformacionArtistaAsync();
                    artistaRegistradoEnBD = await Cliente_MusiCloud.artista.aplicacion.Aplicacion.RegistrarArtista(artista);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            return artistaRegistradoEnBD;
        }
        private bool ValidarCampos()
        {
            if (String.IsNullOrEmpty(txt_NombreArtista.Text) && (String.IsNullOrEmpty(txt_DescripcionArtista.Text)))
            {
                MessageBox.Show("Favor de ingresar información en todos los campos", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        private async Task<Artista> ObtenerInformacionArtistaAsync()
        {
            Artista artista = new Artista
            {
                nombre = txt_NombreArtista.Text,
                descripcion = txt_DescripcionArtista.Text,
                fechaRegistro = DateTime.Now,
                portada = ObtenerPortada(),
                idGenero = await ObtenerIdGeneroAsync()
            };
            return artista;
   
        }

        private String ObtenerPortada()
        {
            string portadaArtista;
            if (pathAbsolutoImagen != null)
            {
                return portadaArtista = CodificacionImagenes.CodificarBase64(pathAbsolutoImagen);
            }
            return portadaArtista = "";
        }

        private async Task<int> ObtenerIdGeneroAsync()
        {
            int idGenero = -1;
            try
            {
                string nombreGenero = CoBox_Generos.SelectedItem.ToString();
                List<Genero> listaId =  await AplicacionGenero.ObtenerIdGeneroAsync(nombreGenero);
                foreach (var listGe in listaId)
                {
                    idGenero = listGe.idGenero;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
   
            }
            return idGenero;
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
            NavigationService.Navigate(new ModificarCuenta());
        }

    }
}
