using System.Collections.Generic;
using PruebaTecnica.Core.DataProviders.AccesoDatos.DataContext.Contratos;
using PruebaTecnica.Core.DataProviders.Entity.Seguridad;

namespace PruebaTecnica.Core.DataProviders.AccesoDatos.DataContext.Contratos
{
    public interface IUsuarioRepository: IBaseRepository<Usuario>
    {
        Usuario Obtener(int id, bool includeRelatedEntities = true);
        IList<Usuario> ObtenerLista();
        
    }
}