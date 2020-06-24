using System;
namespace PruebaTecnica.Core.DataProviders.AccesoDatos.DataContext.Contratos
{
    public class InicializarDB
    { 
        public static void Inicializar(Context context)
        {
            context.Database.EnsureCreated();
        }
    }
}
