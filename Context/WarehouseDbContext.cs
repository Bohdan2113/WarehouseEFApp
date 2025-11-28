using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WarehouseEFApp.Models;

namespace WarehouseEFApp.Context;

public partial class WarehouseDbContext : DbContext
{
    public WarehouseDbContext(DbContextOptions<WarehouseDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Availability> Availabilities { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<DeliveryDocument> DeliveryDocuments { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ShippingDocument> ShippingDocuments { get; set; }

    public virtual DbSet<TransactionLine> TransactionLines { get; set; }

    public virtual DbSet<TransferDocument> TransferDocuments { get; set; }

    public virtual DbSet<VProductMovementAnalysis> VProductMovementAnalyses { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("gender_enum", new[] { "M", "F", "O" })
            .HasPostgresEnum("transaction_type_enum", new[] { "DELIVERY", "SHIPPING", "TRANSFER" });

        modelBuilder.Entity<Availability>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("availability_pkey");

            entity.ToTable("availability");

            entity.HasIndex(e => new { e.ProductId, e.WarehouseId }, "availability_unique").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(12, 3)
                .HasColumnName("quantity");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Availabilities)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("availability_product_id_fkey");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Availabilities)
                .HasForeignKey(d => d.WarehouseId)
                .HasConstraintName("availability_warehouse_id_fkey");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("category_pkey");

            entity.ToTable("category");

            entity.HasIndex(e => e.Name, "category_name_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<DeliveryDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("delivery_document_pkey");

            entity.ToTable("delivery_document");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.DeliveryDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("delivery_date");
            entity.Property(e => e.Supplier).HasColumnName("supplier");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.SupplierNavigation).WithMany(p => p.DeliveryDocuments)
                .HasForeignKey(d => d.Supplier)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("delivery_document_supplier_fkey");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.DeliveryDocuments)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("delivery_document_warehouse_id_fkey");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("person_pkey");

            entity.ToTable("person");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Position)
                .HasMaxLength(100)
                .HasColumnName("position");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pkey");

            entity.ToTable("product");

            entity.HasIndex(e => new { e.Name, e.CategoryId }, "product_name_cat_unique").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.DateAdded)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("date_added");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Unit)
                .HasMaxLength(20)
                .HasColumnName("unit");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("product_category_id_fkey");
        });

        modelBuilder.Entity<ShippingDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("shipping_document_pkey");

            entity.ToTable("shipping_document");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Recipient).HasColumnName("recipient");
            entity.Property(e => e.ShippingDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("shipping_date");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.RecipientNavigation).WithMany(p => p.ShippingDocuments)
                .HasForeignKey(d => d.Recipient)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("shipping_document_recipient_fkey");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.ShippingDocuments)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("shipping_document_warehouse_id_fkey");
        });

        modelBuilder.Entity<TransactionLine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transaction_line_pkey");

            entity.ToTable("transaction_line");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity)
                .HasPrecision(12, 3)
                .HasColumnName("quantity");
            entity.Property(e => e.TransactionId).HasColumnName("transaction_id");

            entity.HasOne(d => d.Product).WithMany(p => p.TransactionLines)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("transaction_line_product_id_fkey");
        });

        modelBuilder.Entity<TransferDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transfer_document_pkey");

            entity.ToTable("transfer_document");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.ReceivingWarehouseId).HasColumnName("receiving_warehouse_id");
            entity.Property(e => e.SourceWarehouseId).HasColumnName("source_warehouse_id");
            entity.Property(e => e.TransferDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("transfer_date");

            entity.HasOne(d => d.ReceivingWarehouse).WithMany(p => p.TransferDocumentReceivingWarehouses)
                .HasForeignKey(d => d.ReceivingWarehouseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("transfer_document_receiving_warehouse_id_fkey");

            entity.HasOne(d => d.SourceWarehouse).WithMany(p => p.TransferDocumentSourceWarehouses)
                .HasForeignKey(d => d.SourceWarehouseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("transfer_document_source_warehouse_id_fkey");
        });

        modelBuilder.Entity<VProductMovementAnalysis>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("v_product_movement_analysis");

            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .HasColumnName("category_name");
            entity.Property(e => e.CriticalDeliveriesQty).HasColumnName("critical_deliveries_qty");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ProductName)
                .HasMaxLength(150)
                .HasColumnName("product_name");
            entity.Property(e => e.TotalDelivered).HasColumnName("total_delivered");
            entity.Property(e => e.TotalShipped).HasColumnName("total_shipped");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("warehouse_pkey");

            entity.ToTable("warehouse");

            entity.HasIndex(e => e.Name, "warehouse_name_key").IsUnique();

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.ResponsiblePersonId).HasColumnName("responsible_person_id");

            entity.HasOne(d => d.ResponsiblePerson).WithMany(p => p.Warehouses)
                .HasForeignKey(d => d.ResponsiblePersonId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("warehouse_responsible_person_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
