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
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Transfer> Transfers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder
        .Entity<CartItem>()
        .HasKey(e => new { e.CartId, e.ItemId });

      builder.Entity<Item>().ToTable("Items");
      builder.Entity<ItemCharacteristics>().ToTable("Items");

      builder
        .Entity<ApplicationUser>()
        .HasOne(e => e.Cart)
        .WithOne(e => e.User)
        .HasForeignKey<Cart>(e => e.UserId)
        .OnDelete(DeleteBehavior.Cascade);

      builder
        .Entity<ApplicationUser>()
        .HasMany(e => e.Orders)
        .WithOne(e => e.User)
        .HasForeignKey(e => e.UserId)
        .OnDelete(DeleteBehavior.Cascade);

      builder
        .Entity<ApplicationUser>()
        .HasMany(e => e.TransfersAsSender)
        .WithOne(e => e.UserSender)
        .HasForeignKey(e => e.UserSenderId)
        .OnDelete(DeleteBehavior.Restrict);

      builder
        .Entity<ApplicationUser>()
        .HasMany(e => e.TransfersAsReceiver)
        .WithOne(e => e.UserReceiver)
        .HasForeignKey(e => e.UserReceiverId)
        .OnDelete(DeleteBehavior.Restrict);

      builder
        .Entity<Cart>()
        .HasOne(e => e.Order)
        .WithOne(e => e.Cart)
        .HasForeignKey<Order>(e => e.CartId)
        .OnDelete(DeleteBehavior.Restrict);
    }
  }
}
