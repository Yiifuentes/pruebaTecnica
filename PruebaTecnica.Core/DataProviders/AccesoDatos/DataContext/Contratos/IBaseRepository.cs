using System.Collections.Generic;

namespace PruebaTecnica.Core.DataProviders.AccesoDatos.DataContext.Contratos
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        void Actualizar(TEntity entity);
        void Crear(TEntity entity);
        void Eliminar(TEntity entity);
        TEntity Obtener(string id, bool includeRelatedEntities = true);
        IList<TEntity> ObtenerLista();
    }
}