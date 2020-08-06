using Cliente_MusiCloud.cuenta.Dominio;
using Cliente_MusiCloud.cuenta.LoginRR;
using Cliente_MusiCloud.Cuenta;
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
    /// Lógica de interacción para ModificarCuenta.xaml
    /// </summary>
    public partial class ModificarCuenta : Page
    {
        Cuentas cuenta = SingletonCuenta.GetSingletonCuenta();
        Cuentas nuevaCuenta;
        
        public ModificarCuenta()
        {
            InitializeComponent();
            ConfiguracionInicialCampos();
            CargarInformacionCampos();
            
        }

        private void Btn_Regresar_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Home());
        }

        private async void btn_ConvertirseEnCreador_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Está seguro de configurar su cuenta como Creador de Contenido?", "Convertirse en creador", MessageBoxButton.OKCancel);
            if (messageBoxResult == MessageBoxResult.OK)
            {
                if (cuenta.creadorContenido == false)
                {
                    try
                    {
                        string respuesta = await Aplicacion.ConvertirseEnCreadorDeContenido(cuenta.idCuenta);
                        MessageBox.Show(respuesta, "Operación exitosa");
                        NavigationService.Navigate(new Home());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Ya eres creador de contenido", "Operación fallida");
                }
            }
        }

        private async void btn_GuardarCambios_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarCampos() && ValidarContraseñasIguales())
            {
                if (ValidarAccionGuardar())
                {
                    nuevaCuenta = ObtenerNuevosDatosCuenta();
                    try
                    {
                        bool mensaje = await Aplicacion.ActualizarCuenta(nuevaCuenta);
                        if (mensaje)
                        {
                            MessageBox.Show("Se han actualizado los datos", "Operación exitosa");
                            SingletonCuenta.SetCuenta(nuevaCuenta);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ocurrió un error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }
        private Cuentas ObtenerNuevosDatosCuenta()
        {
            Cuentas nuevaCuenta = new Cuentas
            {
                idCuenta = cuenta.idCuenta,
                nombre = txt_Nombre.Text,
                apellidos = txt_Apellidos.Text,
                contraseña = txt_contraseña.Password,
                nombreUsuario = cuenta.nombreUsuario,
                correo = cuenta.correo,
                creadorContenido = cuenta.creadorContenido
            };
            
            return nuevaCuenta;
        }
        private bool ValidarCampos()
        {
            if (String.IsNullOrEmpty(txt_NombreUsuario.Text) || String.IsNullOrEmpty(txt_Nombre.Text) || String.IsNullOrEmpty(txt_Apellidos.Text) 
                || String.IsNullOrEmpty(txt_contraseña.Password))
            {
                MessageBox.Show("Favor de ingresar información en todos los campos", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }
        private bool ValidarContraseñasIguales()
        {
            if (txt_contraseña.Password.Equals(txt_Confirmarcontraseña.Password))
            {
                return true;
            }
            MessageBox.Show("Las contraseñas no coinciden", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }
        private bool ValidarAccionGuardar()
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("¿Está seguro de configurar de modificar la información?", "Convertirse en creador", MessageBoxButton.OKCancel);
            if (messageBoxResult == MessageBoxResult.OK)
            {
                return true;
            }
            return false;
        }
        private void btn_HabilitarCampos_Click(object sender, RoutedEventArgs e)
        {
            btn_ConvertirseEnCreador.Visibility = Visibility.Visible;
            btn_GuardarCambios.Visibility = Visibility.Visible;
            txt_Creador.Visibility = Visibility.Visible;
            txt_guardar.Visibility = Visibility.Visible;
            btn_HabilitarCampos.IsEnabled = false;
            txt_Nombre.IsEnabled = true;
            txt_Apellidos.IsEnabled = true;
            txt_contraseña.IsEnabled = true;
            txt_Confirmarcontraseña.IsEnabled = true;

        }

        private void ConfiguracionInicialCampos()
        {
            btn_ConvertirseEnCreador.Visibility = Visibility.Hidden;
            btn_GuardarCambios.Visibility = Visibility.Hidden;
            txt_Creador.Visibility = Visibility.Hidden;
            txt_guardar.Visibility = Visibility.Hidden;
            btn_HabilitarCampos.IsEnabled = true;
            txt_Nombre.IsEnabled = false;
            txt_Apellidos.IsEnabled = false;
            txt_NombreUsuario.IsEnabled = false;
            txt_Correo.IsEnabled = false;
            txt_contraseña.IsEnabled = false;
            txt_Confirmarcontraseña.IsEnabled = false;
        }

        private void CargarInformacionCampos()
        {
            txt_Nombre.Text = cuenta.nombre;
            txt_Apellidos.Text = cuenta.apellidos;
            txt_NombreUsuario.Text = cuenta.nombreUsuario;
            txt_Correo.Text = cuenta.correo;
        }
    }
}
