using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SAM.DataTier.Models;

public partial class SamContext : DbContext
{
    public SamContext()
    {
    }

    public SamContext(DbContextOptions<SamContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountRank> AccountRanks { get; set; }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Certification> Certifications { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<DiscountMachinery> DiscountMachineries { get; set; }

    public virtual DbSet<ImagesAll> ImagesAlls { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<MachineComponent> MachineComponents { get; set; }

    public virtual DbSet<MachinePartMachine> MachinePartMachines { get; set; }

    public virtual DbSet<Machinery> Machineries { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<NewsImage> NewsImages { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Origin> Origins { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Rank> Ranks { get; set; }

    public virtual DbSet<Specification> Specifications { get; set; }

    public virtual DbSet<TransactionPayment> TransactionPayments { get; set; }

    public virtual DbSet<Warranty> Warranties { get; set; }

    public virtual DbSet<WarrantyDetail> WarrantyDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=14.225.204.144;Database=SAM;Uid=vinhuser;Pwd=12345;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3213E83F086B1737");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Id, "UQ__Account__3213E83E68399404").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(255);
        });

        modelBuilder.Entity<AccountRank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccountR__3214EC079F9B2174");

            entity.ToTable("AccountRank");

            entity.HasIndex(e => e.Id, "UQ__AccountR__3214EC06DB6D44D9").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithMany(p => p.AccountRanks)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountRank_Account");

            entity.HasOne(d => d.Rank).WithMany(p => p.AccountRanks)
                .HasForeignKey(d => d.RankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountRank_Rank");
        });

        modelBuilder.Entity<Area>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Area__3214EC07A29447FC");

            entity.ToTable("Area");

            entity.HasIndex(e => e.Id, "UQ__Area__3214EC068CA0ED2E").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(255);
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.ToTable("Brand");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Urlimage)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("URLImage");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3213E83F7BE3BF50");

            entity.ToTable("Category");

            entity.HasIndex(e => e.Id, "UQ__Category__3213E83EEA87B891").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(255);

            entity.HasOne(d => d.MasterCategory).WithMany(p => p.InverseMasterCategory)
                .HasForeignKey(d => d.MasterCategoryId)
                .HasConstraintName("FK_Category_MasterCategory");
        });

        modelBuilder.Entity<Certification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Certific__3213E83F64018812");

            entity.HasIndex(e => e.Id, "UQ__Certific__3213E83E23FA8FE2").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CertificationLink).HasMaxLength(4000);
            entity.Property(e => e.DateObtained).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Certifications)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Certifications_Account");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Discount__3214EC076BF8A8B1");

            entity.ToTable("Discount");

            entity.HasIndex(e => e.Id, "UQ__Discount__3214EC06339CFF00").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Type).HasMaxLength(250);
        });

        modelBuilder.Entity<DiscountMachinery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Discount__3214EC076F9C4689");

            entity.ToTable("DiscountMachinery");

            entity.HasIndex(e => e.Id, "UQ__Discount__3214EC06E4C28458").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Discount).WithMany(p => p.DiscountMachineries)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("FK_DiscountMachinery_Discount");

            entity.HasOne(d => d.Machinery).WithMany(p => p.DiscountMachineries)
                .HasForeignKey(d => d.MachineryId)
                .HasConstraintName("FK_DiscountMachinery_Machinery");
        });

        modelBuilder.Entity<ImagesAll>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Images__3214EC074E69D041");

            entity.ToTable("ImagesAll");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ImageURL");

            entity.HasOne(d => d.Machinery).WithMany(p => p.ImagesAlls)
                .HasForeignKey(d => d.MachineryId)
                .HasConstraintName("FK__Images__Machiner__45BE5BA9");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inventor__3214EC0771CF7A52");

            entity.ToTable("Inventory");

            entity.HasIndex(e => e.Id, "UQ__Inventor__3214EC068D7C4137").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.SerialNumber).HasMaxLength(255);
            entity.Property(e => e.SoldDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Machinery).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.MachineryId)
                .HasConstraintName("FK_Inventory_Machinery");
        });

        modelBuilder.Entity<MachineComponent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MachineC__3213E83F77140DB9");

            entity.HasIndex(e => e.Id, "UQ__MachineC__3213E83E4C386BCF").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<MachinePartMachine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MachineP__3214EC07DFE70D19");

            entity.HasIndex(e => e.Quantity, "UQ__MachineP__DC4401B25E041C9A").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Status).HasMaxLength(255);

            entity.HasOne(d => d.MachineComponent).WithMany(p => p.MachinePartMachines)
                .HasForeignKey(d => d.MachineComponentId)
                .HasConstraintName("FK_MachinePartMachines_MachineComponents");

            entity.HasOne(d => d.Machinery).WithMany(p => p.MachinePartMachines)
                .HasForeignKey(d => d.MachineryId)
                .HasConstraintName("FK_MachinePartMachines_Machinery");
        });

        modelBuilder.Entity<Machinery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Machiner__3213E83F9BB76D03");

            entity.ToTable("Machinery");

            entity.HasIndex(e => e.Id, "UQ__Machiner__3213E83E14DE8756").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.Model)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(255);

            entity.HasOne(d => d.Brand).WithMany(p => p.Machineries)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK_Machinery_Brand");

            entity.HasOne(d => d.Category).WithMany(p => p.Machineries)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Machinery_Category");

            entity.HasOne(d => d.Origin).WithMany(p => p.Machineries)
                .HasForeignKey(d => d.OriginId)
                .HasConstraintName("FK_Machinery_Origin");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__News__3214EC0778DA6714");

            entity.HasIndex(e => e.Id, "UQ__News__3214EC06084685DB").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Cover).HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.NewsContent).HasMaxLength(4000);
            entity.Property(e => e.Title).HasMaxLength(4000);

            entity.HasOne(d => d.Machinery).WithMany(p => p.News)
                .HasForeignKey(d => d.MachineryId)
                .HasConstraintName("FK_News_Machinery");
        });

        modelBuilder.Entity<NewsImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NewsImag__3214EC0706006152");

            entity.ToTable("NewsImage");

            entity.HasIndex(e => e.Id, "UQ__NewsImag__3214EC0630722A48").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ImgUrl).HasMaxLength(4000);
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.News).WithMany(p => p.NewsImages)
                .HasForeignKey(d => d.NewsId)
                .HasConstraintName("FK_NewsImage_News");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3213E83FA756148A");

            entity.ToTable("Order");

            entity.HasIndex(e => e.Id, "UQ__Order__3213E83E5D13AB06").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CompletedDate).HasColumnType("datetime");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceCode).HasMaxLength(255);
            entity.Property(e => e.Note).HasMaxLength(4000);
            entity.Property(e => e.Status).HasMaxLength(255);

            entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Order_Account");

            entity.HasOne(d => d.Area).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK_Order_Area");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3213E83F8642C5B0");

            entity.ToTable("OrderDetail");

            entity.HasIndex(e => e.Id, "UQ__OrderDet__3213E83E76775357").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Inventory).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.InventoryId)
                .HasConstraintName("FK_OrderDetail_Inventory");

            entity.HasOne(d => d.Machinery).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.MachineryId)
                .HasConstraintName("FK_OrderDetail_Machinery");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderDetail_Order");
        });

        modelBuilder.Entity<Origin>(entity =>
        {
            entity.ToTable("Origin");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC076CC98BFD");

            entity.ToTable("Payment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaymentDate).HasColumnType("date");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_Payment_Order");
        });

        modelBuilder.Entity<Rank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rank__3214EC0743A197B9");

            entity.ToTable("Rank");

            entity.HasIndex(e => e.Id, "UQ__Rank__3214EC06B0ABDC2C").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(4000);
        });

        modelBuilder.Entity<Specification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Specific__3213E83FC7C484F7");

            entity.HasIndex(e => e.Id, "UQ__Specific__3213E83EFEF4EEA4").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Value).HasMaxLength(250);

            entity.HasOne(d => d.Machinery).WithMany(p => p.Specifications)
                .HasForeignKey(d => d.MachineryId)
                .HasConstraintName("FK_Specifications_Machinery");
        });

        modelBuilder.Entity<TransactionPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC07F977F44E");

            entity.ToTable("TransactionPayment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransactionDate).HasColumnType("date");

            entity.HasOne(d => d.Payment).WithMany(p => p.TransactionPayments)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK_TransactionPayment_Payment");
        });

        modelBuilder.Entity<Warranty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Warranty__3213E83F1EFA27B6");

            entity.ToTable("Warranty");

            entity.HasIndex(e => e.Id, "UQ__Warranty__3213E83EE0585EB4").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Comments).HasMaxLength(4000);
            entity.Property(e => e.CompletionDate).HasColumnType("datetime");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.NextMaintenanceDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(255);

            entity.HasOne(d => d.Inventory).WithMany(p => p.Warranties)
                .HasForeignKey(d => d.InventoryId)
                .HasConstraintName("FK_Warranty_Inventory");
        });

        modelBuilder.Entity<WarrantyDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Warranty__3213E83F4FF2A95E");

            entity.ToTable("WarrantyDetail");

            entity.HasIndex(e => e.Id, "UQ__Warranty__3213E83E2C58FC35").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Comments).HasMaxLength(4000);
            entity.Property(e => e.CompletionDate).HasColumnType("datetime");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.NextMaintenanceDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(255);

            entity.HasOne(d => d.Account).WithMany(p => p.WarrantyDetails)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_WarrantyDetail_Account");

            entity.HasOne(d => d.Warranty).WithMany(p => p.WarrantyDetails)
                .HasForeignKey(d => d.WarrantyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WarrantyDetail_Warranty");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
