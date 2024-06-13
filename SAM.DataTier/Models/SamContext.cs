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

    public virtual DbSet<ImagesAll> ImagesAlls { get; set; }

    public virtual DbSet<MachineComponent> MachineComponents { get; set; }

    public virtual DbSet<MachinePartMachine> MachinePartMachines { get; set; }

    public virtual DbSet<Machinery> Machineries { get; set; }

    public virtual DbSet<MachinerySpecification> MachinerySpecifications { get; set; }

    public virtual DbSet<Maintenance> Maintenances { get; set; }

    public virtual DbSet<MaintenanceDetail> MaintenanceDetails { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Rank> Ranks { get; set; }

    public virtual DbSet<Specification> Specifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);Database=SAM;Uid=sa;Pwd=12345;TrustServerCertificate=True");

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
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .IsUnicode(false);
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
            entity.HasKey(e => e.Id).HasName("PK__Category__3213E83F393C175D");

            entity.ToTable("Category");

            entity.HasIndex(e => e.Id, "UQ__Category__3213E83E1B55FAB3").IsUnique();

            entity.HasIndex(e => e.Id, "UQ__Category__3213E83EEAA7491B").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(255);

            entity.HasOne(d => d.MasterCategory).WithMany(p => p.InverseMasterCategory)
                .HasForeignKey(d => d.MasterCategoryId)
                .HasConstraintName("FK_Category_MasterCategory");
        });

        modelBuilder.Entity<Certification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Certific__3213E83F77F08C71");

            entity.HasIndex(e => e.Id, "UQ__Certific__3213E83E976F3F26").IsUnique();

            entity.HasIndex(e => e.Id, "UQ__Certific__3213E83ED6B0368A").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CertificationLink).HasMaxLength(255);
            entity.Property(e => e.DateObtained).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Certifications)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Certifications_Account");
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

        modelBuilder.Entity<MachineComponent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MachineC__3213E83FF0C6F7C4");

            entity.HasIndex(e => e.Id, "UQ__MachineC__3213E83EC4AB2828").IsUnique();

            entity.HasIndex(e => e.Id, "UQ__MachineC__3213E83ECE028A8E").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<MachinePartMachine>(entity =>
        {
            entity.HasKey(e => new { e.MachineComponentId, e.MachineryId }).HasName("PK__MachineP__274A8E5B4A303BF0");

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
            entity.Property(e => e.Brand).HasMaxLength(50);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Model).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Origin).HasMaxLength(255);
            entity.Property(e => e.SerialNumber).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(255);

            entity.HasOne(d => d.Category).WithMany(p => p.Machineries)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Machinery_Category");
        });

        modelBuilder.Entity<MachinerySpecification>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Machinery).WithMany(p => p.MachinerySpecifications)
                .HasForeignKey(d => d.MachineryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MachinerySpecifications_Machinery");

            entity.HasOne(d => d.Specifications).WithMany(p => p.MachinerySpecifications)
                .HasForeignKey(d => d.SpecificationsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MachinerySpecifications_Specifications");
        });

        modelBuilder.Entity<Maintenance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Maintena__3213E83FE59F2BB9");

            entity.ToTable("Maintenance");

            entity.HasIndex(e => e.Id, "UQ__Maintena__3213E83E548C3240").IsUnique();

            entity.HasIndex(e => e.Id, "UQ__Maintena__3213E83EF002431C").IsUnique();

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
            entity.HasKey(e => e.Id).HasName("PK__Maintena__3213E83F7EE66E4B");

            entity.ToTable("MaintenanceDetail");

            entity.HasIndex(e => e.Id, "UQ__Maintena__3213E83E0C7482FB").IsUnique();

            entity.HasIndex(e => e.Id, "UQ__Maintena__3213E83E2F186A36").IsUnique();

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
            entity.HasKey(e => e.Id).HasName("PK__Payment__3213E83F4D2355DE");

            entity.ToTable("Payment");

            entity.HasIndex(e => e.Id, "UQ__Payment__3213E83E36AEDBF4").IsUnique();

            entity.HasIndex(e => e.Id, "UQ__Payment__3213E83ED90A51BB").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.PaymentMethod).HasMaxLength(255);
        });

        modelBuilder.Entity<Rank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rank__3213E83F32DB8CB4");

            entity.ToTable("Rank");

            entity.HasIndex(e => e.Id, "UQ__Rank__3213E83E789336F2").IsUnique();

            entity.HasIndex(e => e.Id, "UQ__Rank__3213E83ED5FD52AC").IsUnique();

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
            entity.Property(e => e.Value).HasMaxLength(250);

            entity.HasOne(d => d.Category).WithMany(p => p.Specifications)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Specifications_Category");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
