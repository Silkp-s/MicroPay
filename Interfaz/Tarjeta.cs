using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Avance.Interfaz
{
    public class Tarjeta
    {
        public int NumeroTarjeta { set;get; }   
        public int CCV { set;get; } 
        public DateTime FechaVencimiento { set;get; }

    }
}
