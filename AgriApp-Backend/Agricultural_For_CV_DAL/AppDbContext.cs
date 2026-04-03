using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Agricultural_For_CV_DAL.Entities;
using Microsoft.EntityFrameworkCore;
using MassTransit;


namespace Agricultural_For_CV_DAL
{
    public class AppDbContext:DbContext
    {

            
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        

        public DbSet<Notification> Notifications { get; set; } = null;
        public DbSet<User> Users { get; set; } = null;
        public DbSet<Crop> Crops { get; set; } = null;
        public DbSet<Category> Categories { get; set; } = null;


        public DbSet<Product> Products { get; set; }   
        public DbSet<QuantityTypes> QuantityTypes { get; set; }
        public DbSet<ProductsImages> ProductsImages { get; set; }
        public DbSet<Order> Orders{ get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<AuditLogs> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.AddTransactionalOutboxEntities();
            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddTransactionalOutboxEntities();

            // -------------------
            // User Entity Config
            // -------------------

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
                entity.Property(u=>u.Email).IsRequired().HasMaxLength(200); 
                entity.Property(u=>u.PasswordHash).IsRequired().HasMaxLength(50); 

            });


            // -------------------
            // Category Entity Config
            // -------------------


            modelBuilder.Entity<Category>(entity =>
            {


                entity.Property(c => c.Name).IsRequired().HasMaxLength(50);
                entity.HasIndex(c => c.Name).IsUnique(); // Name فريد


            });


            // -------------------
            // Crop Entity Config
            // -------------------

            modelBuilder.Entity<Crop>(entity =>
            {

                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);

                // the relation with User
                entity.HasOne(c => c.Owner)
                      .WithMany(u => u.Crops)
                      .HasForeignKey(c => c.OwnerId)
                      .OnDelete(DeleteBehavior.NoAction);


                // the relation with Category
                entity.HasOne(c => c.Category)
                      .WithMany(cate => cate.Crops)
                      .HasForeignKey(c => c.CategoryId)
                      .OnDelete(DeleteBehavior.NoAction);

            });


            // -------------------
            // Product Entity Config
            // -------------------



            modelBuilder.Entity<Product>(entity =>
            {
                
                //with crops
                entity.HasOne(p => p.Crops)
                      .WithMany(c => c.Product)
                      .HasForeignKey(p => p.CropTypeId)
                      .OnDelete(DeleteBehavior.NoAction);


                //with QuantityTypes
                entity.HasOne(p=>p.QuantityTypes)
                      .WithMany(q =>q.Product)
                      .HasForeignKey(p=>p.QuantityTypeId)
                      .OnDelete(DeleteBehavior.NoAction);


                //with Users
                entity.HasOne(p => p.User)
                  .WithMany(u => u.Product)
                  .HasForeignKey(p => p.CreatedBy)
                  .OnDelete(DeleteBehavior.NoAction);
            
            });

            // -------------------
            // ProductsImages Entity Config
            // -------------------
            
            modelBuilder.Entity<ProductsImages>(entity =>
            {

                //with Product
                entity.HasOne(img => img.Product)
                      .WithMany(p => p.ProductsImages)
                      .HasForeignKey(img => img.ProductId)
                      .OnDelete(DeleteBehavior.NoAction);
            });



            // -------------------
            // QuantityTypes Entity Config
            // -------------------
            modelBuilder.Entity<QuantityTypes>(entity =>
            {
                entity.Property(q => q.TypeName).IsRequired().HasMaxLength(200);
                entity.HasIndex(q => q.TypeName).IsUnique();
            });



            // -------------------
            // Ordre Entity Config
            // -------------------
            modelBuilder.Entity<Order>()
              .HasOne(o => o.User)
              .WithMany(u=>u.Orders)
              .HasForeignKey(o => o.CustomerId)
              .OnDelete(DeleteBehavior.NoAction);



            // -------------------
            // OrderDetail Entity Config
            // -------------------
            modelBuilder.Entity<OrderDetail>()
             .HasOne(d => d.Order)
             .WithMany(o => o.OrderDetails)
             .HasForeignKey(d => d.OrderId)
             .OnDelete(DeleteBehavior.NoAction);


            modelBuilder.Entity<OrderDetail>()
                .HasOne(d => d.Product)
                .WithMany(p=>p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(d => d.User)
                .WithMany(u => u.OrderDetails)
                .HasForeignKey(d => d.FarmerId)
                .OnDelete(DeleteBehavior.NoAction);
            

            modelBuilder.Entity<OrderDetail>()
                .Property(d =>d.Total )
                .ValueGeneratedOnAddOrUpdate();
                



            modelBuilder.Entity<AuditLogs>(entity =>
            {

                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ResourceType, e.ResourceId });
                entity.HasIndex(e => e.ActorId);
                entity.HasIndex(e => e.CreatedAt);

                entity.Property(e => e.ActorType).HasMaxLength(50);
                entity.Property(e => e.Action).HasMaxLength(50);
                entity.Property(e => e.ResourceType).HasMaxLength(100);
            });

            // Others
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasPrecision(18, 2); // 18 digits, 2 decimals
            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.UnitPrice)
                .HasPrecision(18, 2);
        }


    }
}
