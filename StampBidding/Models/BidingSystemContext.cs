using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace StampBidding.Models
{
    public partial class BidingSystemContext : DbContext
    {
        public BidingSystemContext()
        {
        }

        public BidingSystemContext(DbContextOptions<BidingSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Auction> Auctions { get; set; } = null!;
        public virtual DbSet<Bidding> Biddings { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<CategoryContent> CategoryContents { get; set; } = null!;
        public virtual DbSet<Item> Items { get; set; } = null!;
        public virtual DbSet<Receipt> Receipts { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=John;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auction>(entity =>
            {
                entity.HasIndex(e => e.CreatorId, "IX_Auctions_CreatorId");

                entity.HasIndex(e => e.StatusId, "IX_Auctions_StatusId");

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Auctions)
                    .HasForeignKey(d => d.CreatorId);

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Auctions)
                    .HasForeignKey(d => d.StatusId);
            });

            modelBuilder.Entity<Bidding>(entity =>
            {
                entity.HasIndex(e => e.ItemId, "IX_Biddings_ItemId");

                entity.HasIndex(e => e.UserId, "IX_Biddings_UserId");

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Biddings)
                    .HasForeignKey(d => d.ItemId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Biddings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasMany(d => d.Items)
                    .WithMany(p => p.Categories)
                    .UsingEntity<Dictionary<string, object>>(
                        "CategoryItem",
                        l => l.HasOne<Item>().WithMany().HasForeignKey("ItemsId"),
                        r => r.HasOne<Category>().WithMany().HasForeignKey("CategoriesId"),
                        j =>
                        {
                            j.HasKey("CategoriesId", "ItemsId");

                            j.ToTable("CategoryItem");

                            j.HasIndex(new[] { "ItemsId" }, "IX_CategoryItem_ItemsId");
                        });
            });

            modelBuilder.Entity<CategoryContent>(entity =>
            {
                entity.HasIndex(e => e.CategoryId, "IX_CategoryContents_CategoryId");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.CategoryContents)
                    .HasForeignKey(d => d.CategoryId);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasIndex(e => e.AuctionId, "IX_Items_AuctionId");

                entity.HasIndex(e => e.BuyerId, "IX_Items_BuyerId");

                entity.HasIndex(e => e.ReceiptId, "IX_Items_ReceiptId");

                entity.HasIndex(e => e.SellerId, "IX_Items_SellerId");

                entity.HasIndex(e => e.StatusId, "IX_Items_StatusId");

                entity.Property(e => e.Name)
                    .HasMaxLength(450)
                    .IsUnicode(false);

                entity.HasOne(d => d.Auction)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.AuctionId);

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.ItemBuyers)
                    .HasForeignKey(d => d.BuyerId);

                entity.HasOne(d => d.Receipt)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.ReceiptId);

                entity.HasOne(d => d.Seller)
                    .WithMany(p => p.ItemSellers)
                    .HasForeignKey(d => d.SellerId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Items)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Receipt>(entity =>
            {
                entity.HasIndex(e => e.BuyerId, "IX_Receipts_BuyerId");

                entity.HasOne(d => d.Buyer)
                    .WithMany(p => p.Receipts)
                    .HasForeignKey(d => d.BuyerId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Uuid);

                entity.HasIndex(e => e.RoleId, "IX_Users_RoleId");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Roles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
