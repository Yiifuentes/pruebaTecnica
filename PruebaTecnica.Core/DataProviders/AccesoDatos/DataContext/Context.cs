using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Core.DataProviders.AccesoDatos.DataContext.Contratos;
using PruebaTecnica.Core.DataProviders.Entity.Seguridad;

namespace PruebaTecnica.Core.DataProviders.AccesoDatos.DataContext
{
    public class Context : DbContext, IContext
    {
        public Context(DbContextOptions<Context> options)
           : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //configurar la propiedad Identificacion para no tener valores duplicados    
            modelBuilder.Entity<Usuario>()
                .HasIndex(k => k.Identificacion)
                .IsUnique();
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<TipoDocumento> TipoDocumentos { get; set; }


    }
}
