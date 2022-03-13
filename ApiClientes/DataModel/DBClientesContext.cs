using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ApiClientes.DataModel
{
    public partial class DBClientesContext : DbContext
    {
        public DBClientesContext()
        {
        }

        public DBClientesContext(DbContextOptions<DBClientesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblApiConfig> TblApiConfigs { get; set; }
        public virtual DbSet<TblCliente> TblClientes { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=DESKTOP-06973TN\\SQLEXPRESS; DataBase=DBClientes; trusted_connection=True");
//            }
//        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblApiConfig>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("tblApiConfig");

                entity.Property(e => e.Mantenimiento)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Metodo)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblCliente>(entity =>
            {
                entity.ToTable("tblCliente");

                entity.HasIndex(e => e.Correo, "IX_tblCliente")
                    .IsUnique();

                entity.Property(e => e.Apellido)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Correo)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
