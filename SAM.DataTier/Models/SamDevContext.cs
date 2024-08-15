using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SAM.DataTier.Models;

public partial class SamDevContext : DbContext
{
    public SamDevContext()
    {
    }

    public SamDevContext(DbContextOptions<SamDevContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountRank> AccountRanks { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Certification> Certifications { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<ComponentChange> ComponentChanges { get; set; }

    public virtual DbSet<Device> Devices { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<DiscountCategory> DiscountCategories { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<Favorite> Favorites { get; set; }

    public virtual DbSet<ImageComponent> ImageComponents { get; set; }

    public virtual DbSet<ImagesAll> ImagesAlls { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<MachineComponent> MachineComponents { get; set; }

    public virtual DbSet<Machinery> Machineries { get; set; }

    public virtual DbSet<MachineryComponentPart> MachineryComponentParts { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<NewsCategory> NewsCategories { get; set; }

    public virtual DbSet<NewsImage> NewsImages { get; set; }

    public virtual DbSet<Note> Notes { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Origin> Origins { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Rank> Ranks { get; set; }

    public virtual DbSet<Specification> Specifications { get; set; }

    public virtual DbSet<TaskManager> TaskManagers { get; set; }

    public virtual DbSet<TransactionPayment> TransactionPayments { get; set; }

    public virtual DbSet<Ward> Wards { get; set; }

    public virtual DbSet<Warranty> Warranties { get; set; }

    public virtual DbSet<WarrantyDetail> WarrantyDetails { get; set; }

    public virtual DbSet<WarrantyNote> WarrantyNotes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=14.225.204.144;Database=SAM_Dev;Uid=vinhuser;Pwd=12345;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Account__3213E83F086B1737");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Id, "UQ__Account__3213E83E68399404").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FcmToken).HasMaxLength(4000);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(50);
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

        modelBuilder.Entity<Address>(entity =>
        {
            entity.ToTable("Address");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.NamePersonal).HasMaxLength(50);
            entity.Property(e => e.Note).HasMaxLength(4000);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Address_Account");

            entity.HasOne(d => d.City).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_Address_City1");

            entity.HasOne(d => d.District).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK_Address_District");

            entity.HasOne(d => d.Ward).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.WardId)
                .HasConstraintName("FK_Address_Ward");
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
            entity.Property(e => e.Kind).HasMaxLength(50);
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

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Cities");

            entity.ToTable("City");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ComponentChange>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_InventoryChange");

            entity.ToTable("ComponentChange");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.HasOne(d => d.MachineComponent).WithMany(p => p.ComponentChanges)
                .HasForeignKey(d => d.MachineComponentId)
                .HasConstraintName("FK_ComponentChange_MachineComponents");

            entity.HasOne(d => d.WarrantyDetail).WithMany(p => p.ComponentChanges)
                .HasForeignKey(d => d.WarrantyDetailId)
                .HasConstraintName("FK_InventoryChange_WarrantyDetail");
        });

        modelBuilder.Entity<Device>(entity =>
        {
            entity.ToTable("Device");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DeviceType)
                .HasMaxLength(550)
                .IsUnicode(false);
            entity.Property(e => e.Fcmtoken)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("FCMToken");
            entity.Property(e => e.LastUpdated).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Devices)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Device_Account");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Discount__3214EC076BF8A8B1");

            entity.ToTable("Discount");

            entity.HasIndex(e => e.Id, "UQ__Discount__3214EC06339CFF00").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Type).HasMaxLength(250);
        });

        modelBuilder.Entity<DiscountCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Discount__3214EC076F9C4689");

            entity.ToTable("DiscountCategory");

            entity.HasIndex(e => e.Id, "UQ__Discount__3214EC06E4C28458").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Category).WithMany(p => p.DiscountCategories)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DiscountCategory_Category");

            entity.HasOne(d => d.Discount).WithMany(p => p.DiscountCategories)
                .HasForeignKey(d => d.DiscountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DiscountCategory_Discount");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Districts");

            entity.ToTable("District");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.City).WithMany(p => p.Districts)
                .HasForeignKey(d => d.CityId)
                .HasConstraintName("FK_District_City");
        });

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.ToTable("Favorite");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Account).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Favorite_Account");

            entity.HasOne(d => d.Machinery).WithMany(p => p.Favorites)
                .HasForeignKey(d => d.MachineryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Favorite_Machinery");
        });

        modelBuilder.Entity<ImageComponent>(entity =>
        {
            entity.ToTable("ImageComponent");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasMaxLength(250);

            entity.HasOne(d => d.MachineComponent).WithMany(p => p.ImageComponents)
                .HasForeignKey(d => d.MachineComponentId)
                .HasConstraintName("FK_ImageComponent_MachineComponents");
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
            entity.Property(e => e.Condition)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.IsRepaired)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.SerialNumber).HasMaxLength(255);
            entity.Property(e => e.SoldDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Machinery).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.MachineryId)
                .HasConstraintName("FK_Inventory_Machinery1");
        });

        modelBuilder.Entity<MachineComponent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MachineC__3213E83F77140DB9");

            entity.HasIndex(e => e.Id, "UQ__MachineC__3213E83E4C386BCF").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Brand).WithMany(p => p.MachineComponents)
                .HasForeignKey(d => d.BrandId)
                .HasConstraintName("FK_MachineComponents_Brand");

            entity.HasOne(d => d.Category).WithMany(p => p.MachineComponents)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_MachineComponents_Category");

            entity.HasOne(d => d.Origin).WithMany(p => p.MachineComponents)
                .HasForeignKey(d => d.OriginId)
                .HasConstraintName("FK_MachineComponents_Origin");
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
                .HasConstraintName("FK_Machinery_Category1");

            entity.HasOne(d => d.Origin).WithMany(p => p.Machineries)
                .HasForeignKey(d => d.OriginId)
                .HasConstraintName("FK_Machinery_Origin");
        });

        modelBuilder.Entity<MachineryComponentPart>(entity =>
        {
            entity.ToTable("MachineryComponentPart");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.MachineComponents).WithMany(p => p.MachineryComponentParts)
                .HasForeignKey(d => d.MachineComponentsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MachineryComponentPart_MachineComponents");

            entity.HasOne(d => d.Machinery).WithMany(p => p.MachineryComponentParts)
                .HasForeignKey(d => d.MachineryId)
                .HasConstraintName("FK_MachineryComponentPart_Machinery");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__News__3214EC0778DA6714");

            entity.HasIndex(e => e.Id, "UQ__News__3214EC06084685DB").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Cover).HasMaxLength(255);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.NewsContent).HasMaxLength(4000);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Title).HasMaxLength(4000);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.News)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_News_Account");

            entity.HasOne(d => d.NewsCategory).WithMany(p => p.News)
                .HasForeignKey(d => d.NewsCategoryId)
                .HasConstraintName("FK_News_NewsCategory");
        });

        modelBuilder.Entity<NewsCategory>(entity =>
        {
            entity.ToTable("NewsCategory");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<NewsImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NewsImag__3214EC0706006152");

            entity.ToTable("NewsImage");

            entity.HasIndex(e => e.Id, "UQ__NewsImag__3214EC0630722A48").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ImgUrl).HasMaxLength(4000);

            entity.HasOne(d => d.News).WithMany(p => p.NewsImages)
                .HasForeignKey(d => d.NewsId)
                .HasConstraintName("FK_NewsImage_News");
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.ToTable("Note");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Order).WithMany(p => p.Notes)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_Note_Order");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notification");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Message).HasMaxLength(4000);
            entity.Property(e => e.NotificationType).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(4000);

            entity.HasOne(d => d.Account).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Notification_Account");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order__3213E83FA756148A");

            entity.ToTable("Order");

            entity.HasIndex(e => e.Id, "UQ__Order__3213E83E5D13AB06").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CompletedDate).HasColumnType("datetime");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.InvoiceCode).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Order_Account");

            entity.HasOne(d => d.Address).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK_Order_Address");
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

            entity.HasOne(d => d.MachineComponent).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.MachineComponentId)
                .HasConstraintName("FK_OrderDetail_MachineComponents");

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
            entity.Property(e => e.Note).HasMaxLength(4000);
            entity.Property(e => e.PaymentDate).HasColumnType("date");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
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

        modelBuilder.Entity<TaskManager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Task");

            entity.ToTable("TaskManager");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CompletedDate).HasColumnType("datetime");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ExcutionDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Account).WithMany(p => p.TaskManagers)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_TaskManager_Account");

            entity.HasOne(d => d.Address).WithMany(p => p.TaskManagers)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("FK_TaskManager_Address");

            entity.HasOne(d => d.Order).WithMany(p => p.TaskManagers)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_TaskManager_Order");

            entity.HasOne(d => d.WarrantyDetail).WithMany(p => p.TaskManagers)
                .HasForeignKey(d => d.WarrantyDetailId)
                .HasConstraintName("FK_TaskManager_WarrantyDetail");
        });

        modelBuilder.Entity<TransactionPayment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC07F977F44E");

            entity.ToTable("TransactionPayment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.InvoiceId).HasMaxLength(50);
            entity.Property(e => e.PayType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransactionJson).HasMaxLength(4000);

            entity.HasOne(d => d.Payment).WithMany(p => p.TransactionPayments)
                .HasForeignKey(d => d.PaymentId)
                .HasConstraintName("FK_TransactionPayment_Payment");
        });

        modelBuilder.Entity<Ward>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Wards");

            entity.ToTable("Ward");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.District).WithMany(p => p.Wards)
                .HasForeignKey(d => d.DistrictId)
                .HasConstraintName("FK_Ward_District");
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

        modelBuilder.Entity<WarrantyNote>(entity =>
        {
            entity.ToTable("WarrantyNote");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.Image).HasMaxLength(500);

            entity.HasOne(d => d.WarrantyDetail).WithMany(p => p.WarrantyNotes)
                .HasForeignKey(d => d.WarrantyDetailId)
                .HasConstraintName("FK_WarrantyNote_WarrantyDetail");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
