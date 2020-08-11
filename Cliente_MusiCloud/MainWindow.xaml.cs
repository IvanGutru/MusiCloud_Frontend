using Cliente_MusiCloud.artista.Dominio;
using Cliente_MusiCloud.artista.aplicacion;
using Cliente_MusiCloud.cuenta.LoginRR;
using Cliente_MusiCloud.Cuenta;
using Cliente_MusiCloud.cuentaArtista.aplicacion;
using Cliente_MusiCloud.cuentaArtista.dominio;
using Cliente_MusiCloud.utilidades;
using System;
using System.Threading.Tasks;
using System.Windows;
using Cliente_MusiCloud.reproductor;

namespace Cliente_MusiCloud
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CuentaArtista CuentaArtista;
        Artista artista;
 
        public MainWindow()
        {
            InitializeComponent();
            ConexionApi.Initialize();
            ServidorReproduccion.ServidorReproduccion.Conectar();
            Reproductor.Initialize();
            
        }

        private async void btn_iniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidarCampos())
                {
                    LoginRequest loginRequest = GetLoginRequest();
                    var loginResponse = await Cuenta.Aplicacion.Login(loginRequest);
                    SingletonCuenta.SetCuenta(loginResponse);
                    CuentaArtista = await ObtenerCuentaArtistaAsync(loginResponse.idCuenta);
                    if (CuentaArtista != null)
                    {
                        artista = await ObtenerArtista(CuentaArtista.idArtista);
                        SingletonArtista.SetArtista(artista);
                    }
                    PaginaPrincipal paginaInicio = new PaginaPrincipal();
                    paginaInicio.Show();
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

        private void btn_registro_Click(object sender, RoutedEventArgs e)
        {
            RegistrarCuenta registrarCuenta = new RegistrarCuenta();
            registrarCuenta.Show();
            this.Close();
        }


        private async Task<CuentaArtista> ObtenerCuentaArtistaAsync(string idCuenta)
        {
            CuentaArtista cuentaArtista = null;
            try
            {
                cuentaArtista = await AplicacionCuentaArtista.ObtenerCuentaArtistaIdCuenta(idCuenta);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return cuentaArtista;
        }

        private async Task<Artista> ObtenerArtista(string idArtista)
        {
            Artista artistaRecuperado = null;
            try
            {
                artistaRecuperado = await Cliente_MusiCloud.artista.aplicacion.Aplicacion.ObtenerArtistaPorId(idArtista);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return artistaRecuperado;
        }
    }
}
