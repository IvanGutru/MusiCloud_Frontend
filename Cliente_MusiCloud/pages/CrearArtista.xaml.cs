using Cliente_MusiCloud.artista.aplicacion;
using Cliente_MusiCloud.artista.Dominio;
using Cliente_MusiCloud.cuentaArtista.aplicacion;
using Cliente_MusiCloud.cuentaArtista.dominio;
using Cliente_MusiCloud.genero.aplicacion;
using Cliente_MusiCloud.genero.dominio;
using Cliente_MusiCloud.utilidades;
using Microsoft.Win32;
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
    /// Lógica de interacción para CrearArtista.xaml
    /// </summary>
    public partial class CrearArtista : Page
    {
        List<Genero> listaGeneros;
        String pathAbsolutoImagen;
        Artista artista;
        String idCuenta = SingletonCuenta.GetSingletonCuenta().idCuenta; 
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
            artista = await RegistrarArtistaAsync();
            if (artista!=null)
            {
                try
                {
                    CuentaArtista cuentaArtista = new CuentaArtista
                    {
                        idCuenta = idCuenta,
                        idArtista = artista.idArtista
                    };
                    bool respuesta = await AplicacionCuentaArtista.RegistrarCuentaArtista(cuentaArtista);
                    if (respuesta)
                    {
                        MessageBox.Show("Artista Registrado con éxito", "Operación éxitosa", MessageBoxButton.OK);
                        NavigationService.Navigate(new Home());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private async Task<Artista> RegistrarArtistaAsync()
        {
            Artista artistaRecuperado = null;
            if (ValidarCampos())
            {
                try
                {
                    artista = await ObtenerInformacionArtistaAsync();
                    artistaRecuperado = await Aplicacion.RegistrarArtista(artista);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Favor de ingresar información en todos los campos", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return artistaRecuperado;
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
        private bool ValidarCampos()
        {
            if (String.IsNullOrEmpty(txt_NombreArtista.Text) && (String.IsNullOrEmpty(txt_DescripcionArtista.Text)))
                return false;
            return true;
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
