﻿using EstateAgentAPI.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace EstateAgentAPI.EF
{
    public partial class EstateAgentContext :DbContext
    {

        public EstateAgentContext() { }

        public EstateAgentContext(DbContextOptions<EstateAgentContext> options) : base(options)
        {
        }

        public virtual DbSet<Buyer> Buyers { get; set; } = null;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Buyer>(entity =>
            {
                entity.ToTable("buyer");
                entity.Property(e => e.Id).HasColumnName("BUYER_ID");
                entity.Property(e => e.FirstName).HasColumnName("FIRST_NAME");
                entity.Property(e => e.Surname).HasColumnName("SURNAME");
                entity.Property(e => e.Address).HasColumnName("ADDRESS");
                entity.Property(e => e.Postcode).HasColumnName("POSTCODE");
                entity.Property(e => e.Phone).HasColumnName("PHONE");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
