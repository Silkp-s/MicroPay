using System.Collections.ObjectModel;
using Microsoft.Data.Sqlite;

namespace Avance.Interfaz;

public partial class Inicio : TabbedPage
{

    private ObservableCollection<string> tarjetas = new ObservableCollection<string>();

    private string databasePath;

    public Inicio()
    {
        InitializeComponent();
        listaTarjetas.ItemsSource = tarjetas;

        databasePath = Path.Combine(FileSystem.AppDataDirectory, "Tarjetas.db");
        //Crea la conexion 
        var strConect = "Data source = Tarjetas.db";
        SqliteConnection connection = new SqliteConnection(strConect);

    }

    private void AgregarTarjeta_Clicked(object sender, EventArgs e)
    {
        string ccv = ccvEntry.Text;
        string numeroTarjeta = numeroTarjetaEntry.Text;
        string fechaVencimiento = fechaVencimientoEntry.Text;

        if (string.IsNullOrWhiteSpace(ccv) || string.IsNullOrWhiteSpace(numeroTarjeta) || string.IsNullOrWhiteSpace(fechaVencimiento))
        {
            DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
        }
        else
        {
            AgregarDatosBaseDeDatos(numeroTarjeta, ccv);

            string nuevaTarjeta = $"CCV: {ccv}, Número de Tarjeta: {numeroTarjeta}, Fecha de Vencimiento: {fechaVencimiento}";
            tarjetas.Add(nuevaTarjeta);
            DisplayAlert("Tarjeta Agregada", "Tarjeta agregada exitosamente.", "OK");
        }
    }

    private void AgregarDatosBaseDeDatos(string numeroTarjeta, string ccv)
    {
        try

        {

            using (var connection = new SqliteConnection($"Data Source={databasePath}"))
            {
                connection.Open();//se abre la conexio

                using (var command = connection.CreateCommand())
                {
                    // Crear la tabla si no existe
                    command.CommandText = @"CREATE TABLE IF NOT EXISTS Tarjeta (
                                          numTarjeta INTEGER PRIMARY KEY,
                                          CCV INTEGER
                                        )";
                    command.ExecuteNonQuery();
                }
            }
            using (var connection = new SqliteConnection($"Data Source={databasePath}"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    // Comandos para insertar datos
                    command.CommandText = "INSERT INTO Tarjeta (numTarjeta, CCV) VALUES (@numTarjeta, @CCV)";
                    command.Parameters.Add("@numTarjeta", SqliteType.Integer);
                    command.Parameters.Add("@CCV", SqliteType.Integer);

                    command.Parameters["@numTarjeta"].Value = Convert.ToInt64(numeroTarjeta);//Inserta el número de la tarjeta por teclado
                    command.Parameters["@CCV"].Value = Convert.ToInt32(ccv);//Inserta el CCV por teclado

                    int filasAfectadas = command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex) // En caso de que no funcione la base de datos muestra el error en un Display Alert 
        {
           
            DisplayAlert("Error", $"Error al agregar datos a la base de datos: {ex.Message}", "OK");
        }
        // En la base de datos datos esta agregada la tarjeta 1,1 de ejemplo
    }
    private void RecargarTarjeta_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(tarjetaNumeroEntry.Text) || string.IsNullOrEmpty(montoRecargaEntry.Text))
        {
            DisplayAlert("Error", "Ingrese el número de tarjeta y el monto de recarga", "OK");
            return;
        }

        // Lógica para procesar la recarga de tarjeta aquí

        DisplayAlert("Recarga Exitosa", $"Se recargó correctamente ${montoRecargaEntry.Text} a la tarjeta {tarjetaNumeroEntry.Text}", "OK");


        tarjetaNumeroEntry.Text = string.Empty;
        montoRecargaEntry.Text = string.Empty;
    }

    private async void Cerrar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();// Boton de Cerrar Sesion

    }
    private void EliminarTarjeta_Clicked(object sender, EventArgs e)
    {
        string numeroTarjetaEliminar = eliminarNumeroTarjetaEntry.Text;

        var tarjetaAEliminar = tarjetas.FirstOrDefault(tarjeta => tarjeta.Contains(numeroTarjetaEliminar)); //Por ahora solo elimina la tarjeta visualmente no de la base de datos 

        if (tarjetaAEliminar != null)
        {
            tarjetas.Remove(tarjetaAEliminar);
            DisplayAlert("Tarjeta Eliminada", "Tarjeta eliminada exitosamente.", "OK");
        }
        else
        {
            DisplayAlert("Error", "La tarjeta no existe.", "OK");
        }

    }
}
