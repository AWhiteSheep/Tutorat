﻿using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Client.Data
{
    public partial class TutoratCoreContext : IdentityDbContext<AspNetUsers>
    {
        public TutoratCoreContext()
        {
        }

        public TutoratCoreContext(DbContextOptions<TutoratCoreContext> options)
            : base(options)
        {
        }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<Demandes> Demandes { get; set; }
        public virtual DbSet<Horraire> Horraire { get; set; }
        public virtual DbSet<Inscriptions> Inscriptions { get; set; }
        public virtual DbSet<Notifications> Notifications { get; set; }
        public virtual DbSet<Services> Services { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Tutorat.Core;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // fait le model creating du parent pour cet les qualificatif de l'utilisateur identity
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasKey(e => new { e.Id });
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");
            });

            modelBuilder.Entity<Demandes>(entity =>
            {
                entity.HasKey(e => new { e.IdentifiantUtilisateur, e.IdentifiantHoraire });

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateExpired).HasDefaultValueSql("(dateadd(day,(1),getdate()))");

                entity.Property(e => e.Notified).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.IdentifiantUtilisateurNavigation)
                    .WithMany(p => p.Demandes)
                    .HasForeignKey(d => d.IdentifiantUtilisateur)
                    .HasConstraintName("FK__Demandes__Identi__37A5467C");
            });

            modelBuilder.Entity<Horraire>(entity =>
            {
                entity.HasKey(e => e.IdentityKey)
                    .HasName("PK__Horraire__796424B85ED5B7FE");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Horraire)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK__Horraire__Servic__38996AB5");
            });

            modelBuilder.Entity<Inscriptions>(entity =>
            {
                entity.HasKey(e => new { e.IdentifiantDemandeur, e.IdentifiantHoraire });

                entity.Property(e => e.AcceptedDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EndDate).HasDefaultValueSql("(dateadd(month,(3),getdate()))");

                entity.HasOne(d => d.IdentifiantDemandeurNavigation)
                    .WithMany(p => p.Inscriptions)
                    .HasForeignKey(d => d.IdentifiantDemandeur)
                    .HasConstraintName("FK__Inscripti__Ident__3A81B327");
            });

            modelBuilder.Entity<Notifications>(entity =>
            {
                entity.HasKey(e => e.IdentityKey)
                    .HasName("PK__Notifica__796424B8FAA2FE6F");

                entity.HasOne(d => d.IdentifiantUtilisateurReceiverNavigation)
                    .WithMany(p => p.NotificationsIdentifiantUtilisateurReceiverNavigation)
                    .HasForeignKey(d => d.IdentifiantUtilisateurReceiver)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notifications_AspNetUsers");

                entity.HasOne(d => d.IdentifiantUtilisateurRelatedNavigation)
                    .WithMany(p => p.NotificationsIdentifiantUtilisateurRelatedNavigation)
                    .HasForeignKey(d => d.IdentifiantUtilisateurRelated)
                    .HasConstraintName("FK_Notifications_RelatedAspNetUser");
            });

            modelBuilder.Entity<Services>(entity =>
            {
                entity.HasKey(e => e.IdentityKey)
                    .HasName("PK__Services__796424B83E787365");

                entity.HasOne(d => d.Tuteur)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.TuteurId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Services_AspNetUsers");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
