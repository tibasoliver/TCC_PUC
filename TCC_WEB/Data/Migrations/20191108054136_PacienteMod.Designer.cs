﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TCC_WEB.Data;

namespace TCC_WEB.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20191108054136_PacienteMod")]
    partial class PacienteMod
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityRole");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("TCC_WEB.Models.Consulta", b =>
                {
                    b.Property<int>("ConsultaId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Data");

                    b.Property<string>("Médico")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<int>("PacienteId");

                    b.Property<int>("horario");

                    b.HasKey("ConsultaId");

                    b.HasIndex("PacienteId");

                    b.ToTable("Consultas");
                });

            modelBuilder.Entity("TCC_WEB.Models.Medicamento", b =>
                {
                    b.Property<int>("MedicamentoId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Fabricante")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.Property<string>("NomeGenerico")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("NomedeFabrica")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("MedicamentoId");

                    b.ToTable("Medicamentos");
                });

            modelBuilder.Entity("TCC_WEB.Models.Paciente", b =>
                {
                    b.Property<int>("PacienteId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("Idade");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("SobreNome")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("PacienteId");

                    b.ToTable("Pacientes");
                });

            modelBuilder.Entity("TCC_WEB.Models.RecebimentodeExame", b =>
                {
                    b.Property<int>("RecebimentodeExameId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Dado")
                        .IsRequired();

                    b.Property<DateTime>("Data");

                    b.Property<int>("PacienteId");

                    b.Property<int>("TipodeExameId");

                    b.Property<bool>("recebe");

                    b.HasKey("RecebimentodeExameId");

                    b.HasIndex("PacienteId");

                    b.HasIndex("TipodeExameId");

                    b.ToTable("RecebimentosdeExame");
                });

            modelBuilder.Entity("TCC_WEB.Models.Receita", b =>
                {
                    b.Property<int>("ReceitaId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Data");

                    b.Property<string>("Dose")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("MedicamentoId");

                    b.Property<string>("Médico")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<int>("PacienteId");

                    b.HasKey("ReceitaId");

                    b.HasIndex("MedicamentoId");

                    b.HasIndex("PacienteId");

                    b.ToTable("Receitas");
                });

            modelBuilder.Entity("TCC_WEB.Models.SolicitacaodeExame", b =>
                {
                    b.Property<int>("SolicitacaodeExameId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Data");

                    b.Property<string>("Médico")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<int>("PacienteId");

                    b.Property<int>("TipodeExameId");

                    b.HasKey("SolicitacaodeExameId");

                    b.HasIndex("PacienteId");

                    b.HasIndex("TipodeExameId");

                    b.ToTable("SolicitacoesdeExame");
                });

            modelBuilder.Entity("TCC_WEB.Models.TipodeExame", b =>
                {
                    b.Property<int>("TipodeExameId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("NomedoExame")
                        .IsRequired()
                        .HasMaxLength(70);

                    b.HasKey("TipodeExameId");

                    b.ToTable("TipodeExame");
                });

            modelBuilder.Entity("TCC_WEB.Models.Usuario", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int>("Idade");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Nome");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("SobreNome");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("TCC_WEB.Models.NivelAcesso", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityRole");

                    b.Property<string>("Descricao");

                    b.Property<string>("Nome");

                    b.ToTable("NivelAcesso");

                    b.HasDiscriminator().HasValue("NivelAcesso");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("TCC_WEB.Models.Usuario")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("TCC_WEB.Models.Usuario")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TCC_WEB.Models.Usuario")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("TCC_WEB.Models.Usuario")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TCC_WEB.Models.Consulta", b =>
                {
                    b.HasOne("TCC_WEB.Models.Paciente", "Paciente")
                        .WithMany("Consultas")
                        .HasForeignKey("PacienteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TCC_WEB.Models.RecebimentodeExame", b =>
                {
                    b.HasOne("TCC_WEB.Models.Paciente", "Paciente")
                        .WithMany("RecebimentosdeExame")
                        .HasForeignKey("PacienteId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TCC_WEB.Models.TipodeExame", "TipodeExame")
                        .WithMany("RecebimentosdeExame")
                        .HasForeignKey("TipodeExameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TCC_WEB.Models.Receita", b =>
                {
                    b.HasOne("TCC_WEB.Models.Medicamento", "Medicamento")
                        .WithMany("Receitas")
                        .HasForeignKey("MedicamentoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TCC_WEB.Models.Paciente", "Paciente")
                        .WithMany("Receitas")
                        .HasForeignKey("PacienteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("TCC_WEB.Models.SolicitacaodeExame", b =>
                {
                    b.HasOne("TCC_WEB.Models.Paciente", "Paciente")
                        .WithMany("SolicitacoesdeExame")
                        .HasForeignKey("PacienteId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("TCC_WEB.Models.TipodeExame", "TipodeExame")
                        .WithMany("SolicitacoesdeExame")
                        .HasForeignKey("TipodeExameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
