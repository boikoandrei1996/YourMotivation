using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ORM.Models;

namespace ORM
{
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
    {
    }

    public DbSet<Item> Items { get; set; }
    public DbSet<ItemCharacteristics> ItemCharacteristics { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Transfer> Transfers { get; set; }
    public DbSet<Order> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);
      // Customize the ASP.NET Identity model and override the defaults if needed.
      // For example, you can rename the ASP.NET Identity table names and more.
      // Add your customizations after calling base.OnModelCreating(builder);

      builder
        .Entity<CartItem>()
        .HasKey(e => new { e.CartId, e.ItemId });

      builder
        .Entity<ApplicationUser>()
        .HasOne(e => e.TempCart)
        .WithOne(e => e.UserOwner)
        .HasForeignKey<Cart>(e => e.UserOwnerId)
        .OnDelete(DeleteBehavior.Restrict);

      builder
        .Entity<Transfer>()
        .HasOne(e => e.UserSender)
        .WithMany(e => e.TransfersAsSender)
        .HasForeignKey(e => e.UserSenderId)
        .OnDelete(DeleteBehavior.Restrict);

      builder
        .Entity<Transfer>()
        .HasOne(e => e.UserReceiver)
        .WithMany(e => e.TransfersAsReceiver)
        .HasForeignKey(e => e.UserReceiverId)
        .OnDelete(DeleteBehavior.Restrict);

      builder
        .Entity<Order>()
        .HasOne(e => e.User)
        .WithMany(e => e.Orders)
        .HasForeignKey(e => e.UserId)
        .OnDelete(DeleteBehavior.Restrict);

      builder
        .Entity<Order>()
        .HasOne(e => e.Cart)
        .WithOne(e => e.Order)
        .HasForeignKey<Cart>(e => e.OrderId)
        .OnDelete(DeleteBehavior.Restrict);
    }
  }
}
