using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TCC_WEB.Models;

namespace TCC_WEB.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<NivelAcesso> NiveisAcessos { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<TipodeExame> TiposdeExame { get; set; }
        public DbSet<Receita> Receitas { get; set; }
        public DbSet<SolicitacaodeExame> SolicitacoesdeExame { get; set; }

        public DbSet<Consulta> Consultas { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<TCC_WEB.Models.TipodeExame> TipodeExame { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Receita>().HasOne(e => e.Medicamento).WithMany(e => e.Receitas).HasForeignKey(e => e.MedicamentoId);
            modelBuilder.Entity<Medicamento>().HasMany(t => t.Receitas).WithOne(t => t.Medicamento);

            modelBuilder.Entity<Receita>().HasOne(e => e.Paciente).WithMany(e => e.Receitas).HasForeignKey(e => e.PacienteId);
            modelBuilder.Entity<Paciente>().HasMany(t => t.Receitas).WithOne(t => t.Paciente);


            modelBuilder.Entity<SolicitacaodeExame>().HasOne(e => e.Paciente).WithMany(e => e.SolicitacoesdeExame).HasForeignKey(e => e.PacienteId);
            modelBuilder.Entity<Paciente>().HasMany(t => t.SolicitacoesdeExame).WithOne(t => t.Paciente);

            modelBuilder.Entity<SolicitacaodeExame>().HasOne(e => e.TipodeExame).WithMany(e => e.SolicitacoesdeExame).HasForeignKey(e => e.TipodeExameId);
            modelBuilder.Entity<TipodeExame>().HasMany(t => t.SolicitacoesdeExame).WithOne(t => t.TipodeExame);

            modelBuilder.Entity<Consulta>().HasOne(e => e.Paciente).WithMany(e => e.Consultas).HasForeignKey(e => e.PacienteId);
            modelBuilder.Entity<Paciente>().HasMany(t => t.Consultas).WithOne(t => t.Paciente);

            modelBuilder.Entity<Consulta>().HasKey(table => new { table.Data, table.horario });

            base.OnModelCreating(modelBuilder);
        }
    }
}
