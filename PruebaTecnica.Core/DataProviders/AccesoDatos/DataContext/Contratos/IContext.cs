using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Core.DataProviders.Entity.Seguridad;

namespace PruebaTecnica.Core.DataProviders.AccesoDatos.DataContext.Contratos
{
    public interface IContext
    {
        DbSet<Usuario> Usuarios { get; set; }
        DbSet<TipoDocumento> TipoDocumentos { get; set; }
    }
}