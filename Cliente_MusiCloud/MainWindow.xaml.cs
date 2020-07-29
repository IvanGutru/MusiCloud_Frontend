using Cliente_MusiCloud.cuenta.LoginRR;
using Cliente_MusiCloud.Cuenta;
using System;
using System.Windows;

namespace Cliente_MusiCloud
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ConexionApi.Initialize();
        }

        private async void btn_iniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidarCampos())
                {
                    LoginRequest loginRequest = GetLoginRequest();
                    var loginResponse = await Aplicacion.Login(loginRequest);
                    Console.WriteLine("La respuesta es");
                    MessageBox.Show(loginResponse.cuenta.correo);
                    PaginaPrincipal paginaPrincipal = new PaginaPrincipal();
                    paginaPrincipal.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Por favor ingrese información en todos los campos");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private LoginRequest GetLoginRequest()
        {
            String correo = txt_email.Text;
            String contrasenia = txt_password.Password;
            LoginRequest loginRequest = new LoginRequest { correo = correo, contraseña = contrasenia };
            return loginRequest;
        }

        private bool ValidarCampos()
        {
            if (String.IsNullOrEmpty(txt_email.Text) || String.IsNullOrEmpty(txt_password.Password))
            {
                return false;
            }
            return true;
        }
    }
}
