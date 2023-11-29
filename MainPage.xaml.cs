using Avance.Interfaz;
using System.Text.Json;

namespace Avance
{
    public partial class MainPage : ContentPage
    {
        private const string FileName = "cuentas.txt";
        private string appData;
        public MainPage()
        {
            InitializeComponent();
            appData = FileSystem.Current.AppDataDirectory;
        }
        
        private async void Iniciar_Clicked(object sender, EventArgs e)
        {
            string usuario = Correo.Text;
            string contrasena = Contrasena.Text;
            if (AutenticarUsuario(usuario, contrasena))
            {
                await Navigation.PushModalAsync(new Inicio());//Boton de Inicio
            }
            else
            {
                // Muestra un mensaje de error si las credenciales son incorrectas.
                await DisplayAlert("Error", "Credenciales incorrectas. Intenta de nuevo.", "Aceptar");
            }
        }
        private bool AutenticarUsuario(string usuario, string contrasena)
        {
            if (File.Exists(appData + '/' + FileName))
            {
                string[] lineas = File.ReadAllLines(appData + '/' + FileName);

                foreach (string linea in lineas)
                {
                    string[] partes = linea.Split(',');

                    if (partes.Length == 2 && partes[0] == usuario && partes[1] == contrasena)
                    {
                        return true;
                    }
                }
            }
            else
            {
                File.CreateText(appData + '/' + FileName);
            }
                // Lee los usuarios del archivo y verifica las credenciales.
            

            return false;
        }

        private async void Crear_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Crear_cuenta()); //Boton de Crear cuenta
        }
        private List<Usuario> LeerUsuariosDesdeJson()
        {
            // Verifica si el archivo JSON existe
            if (File.Exists(appData + '/' + FileName))
            {
                // Lee el contenido del archivo y deserializa la información en una lista de usuarios
                string json = File.ReadAllText(appData + '/' + FileName);
                return JsonSerializer.Deserialize<List<Usuario>>(json);
            }
            else
            {
                // Devuelve una lista vacía si el archivo no existe
                return new List<Usuario>();
            }
        }
    }
}

