using System.Diagnostics.Metrics;
using System.Text.Json;
namespace Avance.Interfaz;

public partial class Crear_cuenta : ContentPage
{
    private List<Usuario> usuarios = new List<Usuario>();
    private string appData = FileSystem.Current.AppDataDirectory;
    public Crear_cuenta()
    {
        InitializeComponent();
    }
    private const string FileName = "cuentas.txt";
    private const string FileUsers = "usuarios.json";
    private void Cuenta_Clicked(object sender, EventArgs e)
    {
        string usuario = Correo.Text;
        string contrasena = Contrasena.Text;


        CrearUsuario(usuario, contrasena);

        // Muestra un mensaje de éxito.
        DisplayAlert("Éxito", "Usuario creado correctamente.", "Aceptar");
    }
    private void CrearUsuario(string usuario, string contrasena)
    {
        File.AppendAllText(appData + '/' + FileName, $"{usuario},{contrasena}\n");

        string rut = Rut.Text; //pasa lo que esta en el textbox a rut
        string nombre = Nombre.Text;
        string apellidoPaterno = ApellidoPat.Text;
        string apellidoMaterno = ApellidoMat.Text;
        int dia = int.Parse(Dia.Text);
        int mes = int.Parse(Mes.Text);
        int anio = int.Parse(Anio.Text);
        string genero = Mas.IsChecked ? "Hombre" : "Mujer";//sirve para comprobar que boton esta seleccionado

        // Crea un objeto usuario
        Usuario nuevoUsuario = new Usuario
        {
            Rut = rut,
            Nombre = nombre,
            ApellidoPaterno = apellidoPaterno,
            ApellidoMaterno = apellidoMaterno,
            FechaNacimiento = new DateTime(anio, mes, dia),
            Genero = genero
        };

        // Agrega el nuevo mesero a la colección
        usuarios.Add(nuevoUsuario);
        GuardarUsuarios();
        // Cada vez que se da en guardar limpia la "terminal" es como cuando se inicializa en 0 cuando no hay nada
        Rut.Text = "";
        Nombre.Text = "";
        ApellidoPat.Text = "";
        ApellidoMat.Text = "";
        Dia.Text = "";
        Mes.Text = "";
        Anio.Text = "";
        Mas.IsChecked = false;//lo que hace esto es desmarcar la opcion
        Fem.IsChecked = false;//lo que hace esto es desmarcar la opcion

    }
    
    private void GuardarUsuarios()
    {
        // Serializa la lista de usuarios a formato JSON y guarda en el archivo.
        string json = JsonSerializer.Serialize(usuarios);
        File.WriteAllText(appData + '/' + FileUsers, json);
    }
    private void CargarUsuarios()
    {
        // Intenta cargar usuarios desde el archivo, si existe.
        if (File.Exists(appData + '/' + FileUsers))
        {
            // Lee el contenido del archivo y deserializa la información en la lista de usuarios
            string json = File.ReadAllText(appData + '/' + FileUsers);
            usuarios = JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
        }
        else
        {
            string json = JsonSerializer.Serialize(usuarios);
            File.WriteAllText(json, appData + "/" + FileUsers);

        }
    }
        private void MostrarDatosButton_Clicked(object sender, EventArgs e)//mostrar ,la base de datos
    {
        if (usuarios.Count > 0)
        {
            string datos = "Datos de Usuario:\n";
            foreach (var usuario in usuarios)
            {
                datos += $"Rut: {usuario.Rut}\n";
                datos += $"Nombre: {usuario.Nombre}\n";
                datos += $"Apellido Paterno: {usuario.ApellidoPaterno}\n";
                datos += $"Apellido Materno: {usuario.ApellidoMaterno}\n";
                datos += $"Fecha de Nacimiento: {usuario.FechaNacimiento.ToShortDateString()}\n";//para el tema de la fecha que no reciba tantos datos
                datos += $"Género: {usuario.Genero}\n";
                datos += "\n";
            }

            DisplayAlert("Datos del Usuario", datos, "OK");
        }
        else
        {
            DisplayAlert("Datos del Usuario", "No hay usuarios guardados.", "OK");
        }
    }

}



