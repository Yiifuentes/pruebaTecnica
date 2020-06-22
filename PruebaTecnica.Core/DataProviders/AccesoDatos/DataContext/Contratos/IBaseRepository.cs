using System.Collections.Generic;

namespace PruebaTecnica.Core.DataProviders.AccesoDatos.DataContext.Contratos
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        void Actualizar(TEntity entity);
        void Crear(TEntity entity);
        void Eliminar(int id);
        TEntity Obtener(int id, bool includeRelatedEntities = true);
        IList<TEntity> ObtenerLista();
    }
}