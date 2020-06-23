using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Core.DataProviders.AccesoDatos.DataContext.Contratos;
using PruebaTecnica.Core.DataProviders.Entity.Seguridad;

namespace PruebaTecnica.Core.DataProviders.AccesoDatos.DataContext
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository,IBaseRepository<Usuario>
    {
        public UsuarioRepository(Context context) : base(context)
        {
        }

        public override Usuario Obtener(string id, bool includeRelatedEntities = true)
        {
            var usuario = Context.Usuarios.AsQueryable();
            if (includeRelatedEntities)
                usuario = usuario.Include(i => i.Identificacion);

             return usuario.Where(e => e.Id == id).FirstOrDefault();
         }
        public Usuario obtenerUsuarioPoIdentificacion(int id)
        {
            return Context.Usuarios.Where(i=>i.Identificacion==id).FirstOrDefault();
        }
        public override IList<Usuario> ObtenerLista()
        {
#pragma warning disable CS1701 // Asumiendo que la referencia al ensamblaje coincide con la identidad
            return Context.Usuarios
                    .Include(t => t.TipoDocumento)
                    .OrderBy(o => o.Nombre)
                    .ToList();
#pragma warning restore CS1701 // Asumiendo que la referencia al ensamblaje coincide con la identidad
        }
    }
}
