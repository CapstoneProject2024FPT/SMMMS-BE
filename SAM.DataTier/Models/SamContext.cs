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

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Certification> Certifications { get; set; }

    public virtual DbSet<MachineComponent> MachineComponents { get; set; }

    public virtual DbSet<MachinePartMachine> MachinePartMachines { get; set; }

    public virtual DbSet<Machinery> Machineries { get; set; }

    public virtual DbSet<Maintenance> Maintenances { get; set; }

    public virtual DbSet<MaintenanceDetail> MaintenanceDetails { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Rank> Ranks { get; set; }

    public virtual DbSet<Specification> Specifications { get; set; }

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

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(255);

            entity.HasOne(d => d.RankNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.Rank)
                .HasConstraintName("FK_Account_Rank");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3213E83F7BE3BF50");

            entity.ToTable("Category");

            entity.HasIndex(e => e.Id, "UQ__Category__3213E83EEA87B891").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(255);

            entity.HasOne(d => d.MasterCategoryNavigation).WithMany(p => p.InverseMasterCategoryNavigation)
                .HasForeignKey(d => d.MasterCategory)
                .HasConstraintName("FK_Category_MasterCategory");
        });

        modelBuilder.Entity<Certification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Certific__3213E83FDF35AFD7");

            entity.HasIndex(e => e.Id, "UQ__Certific__3213E83EF9A93AFB").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CertificationLink).HasMaxLength(255);
            entity.Property(e => e.DateObtained).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Certifications)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Certifications_Account");
        });

        modelBuilder.Entity<MachineComponent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MachineC__3213E83F5465096D");

            entity.HasIndex(e => e.Id, "UQ__MachineC__3213E83E87F82CF5").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<MachinePartMachine>(entity =>
        {
            entity.HasKey(e => new { e.MachineComponentId, e.MachineryId }).HasName("PK__MachineP__274A8E5B58D90F4F");

            entity.Property(e => e.Status).HasMaxLength(255);

            entity.HasOne(d => d.MachineComponent).WithMany(p => p.MachinePartMachines)
                .HasForeignKey(d => d.MachineComponentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MachinePartMachines_MachineComponents");

            entity.HasOne(d => d.Machinery).WithMany(p => p.MachinePartMachines)
                .HasForeignKey(d => d.MachineryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MachinePartMachines_Machinery");
        });

        modelBuilder.Entity<Machinery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Machiner__3213E83F9BB76D03");

            entity.ToTable("Machinery");

            entity.HasIndex(e => e.Id, "UQ__Machiner__3213E83E14DE8756").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.Model).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Origin).HasMaxLength(255);
            entity.Property(e => e.SerialNumber).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(255);

            entity.HasOne(d => d.Category).WithMany(p => p.Machineries)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Machinery_Category");
        });

        modelBuilder.Entity<Maintenance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Maintena__3213E83F6DE72DA8");

            entity.ToTable("Maintenance");

            entity.HasIndex(e => e.Id, "UQ__Maintena__3213E83E3F83DECC").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Comments).HasMaxLength(255);
            entity.Property(e => e.CompletionDate).HasColumnType("datetime");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.NextMaintenanceDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(255);
        });

        modelBuilder.Entity<MaintenanceDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Maintena__3213E83FA173BEC7");

            entity.ToTable("MaintenanceDetail");

            entity.HasIndex(e => e.Id, "UQ__Maintena__3213E83EA2A43B7B").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Comments).HasMaxLength(255);
            entity.Property(e => e.CompletionDate).HasColumnType("datetime");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.NextMaintenanceDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(255);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3213E83FA756148A");

            entity.ToTable("Order");

            entity.HasIndex(e => e.Id, "UQ__Order__3213E83E5D13AB06").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CompletedDate).HasColumnType("datetime");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.InvoiceCode).HasMaxLength(255);
            entity.Property(e => e.Note).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(255);

            entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Order_Account");

            entity.HasOne(d => d.Payment).WithMany(p => p.Orders)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK_Order_Payment");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderDet__3213E83F8642C5B0");

            entity.ToTable("OrderDetail");

            entity.HasIndex(e => e.Id, "UQ__OrderDet__3213E83E76775357").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");

            entity.HasOne(d => d.Machinery).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.MachineryId)
                .HasConstraintName("FK_OrderDetail_Machinery");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderDetail_Order");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payment__3213E83F12FA1E41");

            entity.ToTable("Payment");

            entity.HasIndex(e => e.Id, "UQ__Payment__3213E83EAD52AFA3").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.PaymentMethod).HasMaxLength(255);
        });

        modelBuilder.Entity<Rank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rank__3213E83F232DAFF9");

            entity.ToTable("Rank");

            entity.HasIndex(e => e.Id, "UQ__Rank__3213E83E30B80943").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.RankName).HasMaxLength(255);
        });

        modelBuilder.Entity<Specification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Specific__3213E83FC7C484F7");

            entity.HasIndex(e => e.Id, "UQ__Specific__3213E83EFEF4EEA4").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.Machinery).WithMany(p => p.Specifications)
                .HasForeignKey(d => d.MachineryId)
                .HasConstraintName("FK_Specifications_Machinery");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
