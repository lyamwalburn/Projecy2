
using EstateAgentAPI.Persistence.Models;
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
        public virtual DbSet<Seller> Sellers { get; set; } = null;
        public virtual DbSet<Property> Properties { get; set; } = null;
        public virtual DbSet<Booking> Bookings { get; set; } = null;
        public virtual DbSet<User> Users { get; set; } = null;



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
                entity.Property(e => e.PostCode).HasColumnName("POSTCODE");
                entity.Property(e => e.Phone).HasColumnName("PHONE");
            });

            modelBuilder.Entity<Seller>(entity =>
            {
                entity.ToTable("seller");

                entity.Property(e => e.Id).HasColumnName("SELLER_ID");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .HasColumnName("FIRST_NAME");
                entity.Property(e => e.Surname)
                   .HasMaxLength(255)
                   .HasColumnName("SURNAME");
                entity.Property(e => e.Address)
                   .HasMaxLength(255)
                   .HasColumnName("ADDRESS");
                entity.Property(e => e.PostCode)
                   .HasMaxLength(255)
                   .HasColumnName("POSTCODE");
                entity.Property(e => e.Phone)
                  .HasMaxLength(20)
                  .HasColumnName("PHONE");
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.ToTable("property");
                entity.Property(e => e.Id).HasColumnName("PROPERTY_ID");
                entity.Property(e => e.Address).HasColumnName("ADDRESS");
                entity.Property(e => e.PostCode).HasColumnName("POSTCODE");
                entity.Property(e => e.Type).HasColumnName("TYPE");
                entity.Property(e => e.NumberOfBedrooms).HasColumnName("NUMBER_OF_BEDROOMS");
                entity.Property(e => e.NumberOfBathrooms).HasColumnName("NUMBER_OF_BATHROOMS");
                entity.Property(e => e.Garden).HasColumnName("GARDEN");
                entity.Property(e => e.Price).HasColumnName("PRICE");
                entity.Property(e => e.SellerId).HasColumnName("SELLER_ID");
                entity.Property(e => e.BuyerId).HasColumnName("BUYER_ID");
               

            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("booking");
                entity.Property(e => e.Id).HasColumnName("BOOKING_ID");
                entity.Property(e => e.BuyerId).HasColumnName("BUYER_ID");
                entity.Property(e => e.PropertyId).HasColumnName("PROPERTY_ID");
                entity.Property(e => e.Time).HasColumnName("TIME");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.Property(e => e.Id).HasColumnName("USER_ID");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.UserName).HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
