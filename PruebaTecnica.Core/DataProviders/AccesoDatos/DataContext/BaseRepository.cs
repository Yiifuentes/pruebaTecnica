using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Core.DataProviders.AccesoDatos.DataContext.Contratos;

namespace PruebaTecnica.Core.DataProviders.AccesoDatos.DataContext
{ 

    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        protected Context Context { get; private set; }

        public BaseRepository(Context context)
        {
            Context = context;
        }

        public abstract TEntity Obtener(string id, bool includeRelatedEntities = true);
        public abstract IList<TEntity> ObtenerLista();

        public void Crear(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            Context.SaveChanges();
        }

        public void Actualizar(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
        }

         

        public void Eliminar(TEntity entity)
        {
            Context.Remove(entity);
            Context.SaveChanges();
        }
 
 
    }
}
