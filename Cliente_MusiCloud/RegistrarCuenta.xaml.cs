using Cliente_MusiCloud.cuenta.Dominio;
using Cliente_MusiCloud.Cuenta;
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
using System.Windows.Shapes;

namespace Cliente_MusiCloud
{
    /// <summary>
    /// Lógica de interacción para RegistrarCuenta.xaml
    /// </summary>
    public partial class RegistrarCuenta : Window
    {
        private Cuentas cuenta = new Cuentas();
        public RegistrarCuenta()
        {
            InitializeComponent();
            ConexionApi.Initialize();
        }

        private async void Btn_Registrarse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidarCampos())
                {
                    if (ValidarContraseñasIguales())
                    {
                        cuenta = ObtenerCuenta();
                        bool respuesta = await Aplicacion.CrearCuenta(cuenta);
                        if (respuesta)
                        {
                            MessageBox.Show("Se ha realizado el registro con éxtio");
                            MainWindow mainWindow = new MainWindow();
                            mainWindow.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Las contraseñas no coinciden, vuelva a intentarlo");
                    }
                }
                else
                {
                    MessageBox.Show("Favor de ingresar información en todos los campos");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Btn_Regresar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private bool ValidarCampos()
        {
            if (String.IsNullOrEmpty(txt_usuario.Text) || String.IsNullOrEmpty(txt_correo.Text) || String.IsNullOrEmpty(txt_nombre.Text)
                || String.IsNullOrEmpty(txt_Apellidos.Text) || String.IsNullOrEmpty(txt_Contraseña.Password) || String.IsNullOrEmpty(txt_ConfirmarContraseña.Password))
            {
                return false;
            }
            return true;
        }

        private Cuentas ObtenerCuenta()
        {
            Cuentas cuenta = new Cuentas
            {
                nombreUsuario = txt_usuario.Text,
                correo = txt_correo.Text,
                nombre = txt_nombre.Text,
                apellidos = txt_Apellidos.Text,
                contraseña = txt_Contraseña.Password,
                creadorContenido = false
            };
            return cuenta;
        }
        private bool ValidarContraseñasIguales()
        {
            if (txt_Contraseña.Password.Equals(txt_ConfirmarContraseña.Password))
            {
                return true;
            }
            return false;
        }
    }
}
