using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RenCart.API.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<WishListItem> WishListItems { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppUserRole> AppUserRoles { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrdersDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Category Entity
            builder.Entity<Category>(entity=> {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Name).IsRequired().HasMaxLength(40);
            });
            //Book Entity
            builder.Entity<Book>(entity => {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.CategoryId).IsRequired();
                entity.Property(x => x.Price).IsRequired().HasColumnType("decimal(10,2)");
                entity.Property(x => x.Title).IsRequired().HasMaxLength(100);
                entity.Property(x => x.CoverImage).IsRequired().HasMaxLength(120);
                entity.Property(x => x.CoverImageContent).HasDefaultValue(null);
                entity.Property(x => x.Author).IsRequired().HasMaxLength(100);
            });
            //Appuser
            builder.Entity<AppUser>(entity => {
                entity.HasKey(c => c.Id);
                entity.Property(x => x.FullName).IsRequired().HasMaxLength(50);
                entity.Property(x => x.UserName).IsRequired().HasMaxLength(50);
                entity.Property(x => x.Email).IsRequired().HasMaxLength(60);
                entity.Property(x => x.Password).IsRequired().HasMaxLength(30);
            });
            //1-1 AppUser-WishList
            builder.Entity<AppUser>()
                .HasOne(x => x.WishList)
                .WithOne(c => c.AppUser)
                .HasForeignKey<WishList>(x => x.AppUserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<AppUser>()
                .HasOne(x => x.Cart)
                .WithOne(c => c.AppUser)
                .HasForeignKey<Cart>(x => x.AppUserId)
                .OnDelete(DeleteBehavior.NoAction);
            //builder.Entity<WishList>()
            //    .HasOne(x => x.AppUser)
            //    .WithOne(c => c.WishList)
            //    .HasPrincipalKey<AppUser>(x => x.WishListId)
            //    .HasForeignKey<WishList>(x => x.Id)
            //    .OnDelete(DeleteBehavior.NoAction);
            ////1-1 Appuser-Cart
            //builder.Entity<AppUser>()
            //    .HasOne(x => x.Cart)
            //    .WithOne(c => c.AppUser)
            //    .HasPrincipalKey<AppUser>(x => x.CartId)
            //    .HasForeignKey<Cart>(x => x.Id)
            //    .OnDelete(DeleteBehavior.NoAction);

            //builder.Entity<Cart>()
            //    .HasOne(x => x.AppUser)
            //    .WithOne(c => c.Cart)
            //    .HasPrincipalKey<Cart>(x => x.Id)
            //    .HasForeignKey<AppUser>(x => x.CartId)
            //    .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<WishList>()
                .HasMany(x => x.WishListItems)
                .WithOne(x => x.WishList)
                .HasForeignKey(x=>x.WishListId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Cart>()
                .HasMany(x => x.CartItems)
                .WithOne(x => x.Cart)
                .HasForeignKey(x => x.CartId)
                .OnDelete(DeleteBehavior.NoAction);


            base.OnModelCreating(builder);
        }
    }
}
