using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Common.Classes.DB;

public partial class CoffeeShopContext : DbContext
{
    public CoffeeShopContext()
    {
    }

    public CoffeeShopContext(DbContextOptions<CoffeeShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AllowedIngredients> AllowedIngredients { get; set; }

    public virtual DbSet<CountryCodes> CountryCodes { get; set; }

    public virtual DbSet<Images> Images { get; set; }

    public virtual DbSet<IngredientImages> IngredientImages { get; set; }

    public virtual DbSet<IngredientTypes> IngredientTypes { get; set; }

    public virtual DbSet<Ingredients> Ingredients { get; set; }

    public virtual DbSet<IngredientsView> IngredientsView { get; set; }

    public virtual DbSet<OrderDetailsView> OrderDetailsView { get; set; }

    public virtual DbSet<OrderItemIngredients> OrderItemIngredients { get; set; }

    public virtual DbSet<OrderItems> OrderItems { get; set; }

    public virtual DbSet<OrderProducts> OrderProducts { get; set; }

    public virtual DbSet<OrderStatuses> OrderStatuses { get; set; }

    public virtual DbSet<Orders> Orders { get; set; }

    public virtual DbSet<PaymentStatuses> PaymentStatuses { get; set; }

    public virtual DbSet<ProductImages> ProductImages { get; set; }

    public virtual DbSet<ProductTypes> ProductTypes { get; set; }

    public virtual DbSet<Products> Products { get; set; }

    public virtual DbSet<UserTypes> UserTypes { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-BBFM8MMD\\SQLEXPRESS;Database=CoffeeShop;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AllowedIngredients>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Allowed __3213E83FD728F0D8");

            entity.ToTable("Allowed ingredients");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AllowedNumber).HasColumnName("allowedNumber");
            entity.Property(e => e.IdIngredient).HasColumnName("idIngredient");
            entity.Property(e => e.IdProduct).HasColumnName("idProduct");

            entity.HasOne(d => d.IdIngredientNavigation).WithMany(p => p.AllowedIngredients)
                .HasForeignKey(d => d.IdIngredient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Allowed i__idIng__4A59BEEB");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.AllowedIngredients)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Allowed i__idPro__4B4DE324");
        });

        modelBuilder.Entity<CountryCodes>(entity =>
        {
            entity.HasKey(e => e.CountryTicker).HasName("PK__CountryC__A0F54A7F82824E37");

            entity.Property(e => e.CountryTicker)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("countryTicker");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("countryCode");
            entity.Property(e => e.CountryName)
                .HasMaxLength(100)
                .HasColumnName("countryName");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
        });

        modelBuilder.Entity<Images>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Images__3213E83F0C5C8645");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            entity.Property(e => e.Url)
                .HasMaxLength(1024)
                .HasColumnName("url");
        });

        modelBuilder.Entity<IngredientImages>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ingredie__3213E83F27F27358");

            entity.ToTable("Ingredient images");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.IdImage).HasColumnName("idImage");
            entity.Property(e => e.IdIngredient).HasColumnName("idIngredient");

            entity.HasOne(d => d.IdImageNavigation).WithMany(p => p.IngredientImages)
                .HasForeignKey(d => d.IdImage)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ingredien__idIma__477D5240");

            entity.HasOne(d => d.IdIngredientNavigation).WithMany(p => p.IngredientImages)
                .HasForeignKey(d => d.IdIngredient)
                .HasConstraintName("FK__Ingredien__idIng__46892E07");
        });

        modelBuilder.Entity<IngredientTypes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ingredie__3213E83F3DE9B169");

            entity.ToTable("Ingredient types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Ingredients>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ingredie__3213E83FCC42B8F3");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Fee).HasColumnName("fee");
            entity.Property(e => e.IdIngredientType).HasColumnName("idIngredientType");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.IdIngredientTypeNavigation).WithMany(p => p.Ingredients)
                .HasForeignKey(d => d.IdIngredientType)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Ingredien__idIng__459509CE");
        });

        modelBuilder.Entity<IngredientsView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("IngredientsView");

            entity.Property(e => e.IngredientDescription).HasMaxLength(255);
            entity.Property(e => e.IngredientTitle).HasMaxLength(255);
            entity.Property(e => e.IngredientTypeTitle).HasMaxLength(255);
        });

        modelBuilder.Entity<OrderDetailsView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("OrderDetailsView");

            entity.Property(e => e.CustomerFirstName).HasMaxLength(255);
            entity.Property(e => e.CustomerLastName).HasMaxLength(255);
            entity.Property(e => e.IngredientTitle).HasMaxLength(255);
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PaymentStatus).HasMaxLength(255);
            entity.Property(e => e.ProductTitle).HasMaxLength(255);
        });

        modelBuilder.Entity<OrderItemIngredients>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order it__3213E83FBFEF3274");

            entity.ToTable("Order item ingredients");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.IdIngredient).HasColumnName("idIngredient");
            entity.Property(e => e.IdOrderProduct).HasColumnName("idOrderProduct");

            entity.HasOne(d => d.IdIngredientNavigation).WithMany(p => p.OrderItemIngredients)
                .HasForeignKey(d => d.IdIngredient)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Order ite__idIng__50129841");

            entity.HasOne(d => d.IdOrderProductNavigation).WithMany(p => p.OrderItemIngredients)
                .HasForeignKey(d => d.IdOrderProduct)
                .HasConstraintName("FK__Order ite__idOrd__4F1E7408");
        });

        modelBuilder.Entity<OrderItems>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order it__3213E83F821295C2");

            entity.ToTable("Order items");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdOrder).HasColumnName("idOrder");
            entity.Property(e => e.IdOrderProduct).HasColumnName("idOrderProduct");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.IdOrder)
                .HasConstraintName("FK__Order ite__idOrd__4C42075D");

            entity.HasOne(d => d.IdOrderProductNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.IdOrderProduct)
                .HasConstraintName("FK__Order ite__idOrd__4D362B96");
        });

        modelBuilder.Entity<OrderProducts>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order pr__3213E83F50C51F79");

            entity.ToTable("Order products");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdProduct).HasColumnName("idProduct");
            entity.Property(e => e.Total).HasColumnName("total");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("FK__Order pro__idPro__4E2A4FCF");
        });

        modelBuilder.Entity<OrderStatuses>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderSta__3213E83F34A0D238");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Orders>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3213E83F34185C9C");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("creationDate");
            entity.Property(e => e.IdCustomer).HasColumnName("idCustomer");
            entity.Property(e => e.IdEmployee).HasColumnName("idEmployee");
            entity.Property(e => e.IdStatus)
                .HasDefaultValue(1)
                .HasColumnName("idStatus");
            entity.Property(e => e.IdStatusPayment)
                .HasDefaultValue(1)
                .HasColumnName("idStatusPayment");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.OrdersIdCustomerNavigation)
                .HasForeignKey(d => d.IdCustomer)
                .HasConstraintName("FK__Orders__idCustom__40D054B1");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.OrdersIdEmployeeNavigation)
                .HasForeignKey(d => d.IdEmployee)
                .HasConstraintName("FK__Orders__idEmploy__41C478EA");

            entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__idStatus__42B89D23");

            entity.HasOne(d => d.IdStatusPaymentNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdStatusPayment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__idStatus__43ACC15C");
        });

        modelBuilder.Entity<PaymentStatuses>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PaymentS__3213E83FA5F01FE7");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<ProductImages>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product __3213E83F2ADD4793");

            entity.ToTable("Product images");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdImage).HasColumnName("idImage");
            entity.Property(e => e.IdProduct).HasColumnName("idProduct");

            entity.HasOne(d => d.IdImageNavigation).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.IdImage)
                .HasConstraintName("FK__Product i__idIma__49659AB2");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("FK__Product i__idPro__48717679");
        });

        modelBuilder.Entity<ProductTypes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product __3213E83FB27C54FF");

            entity.ToTable("Product types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Products>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3213E83F4DCA7633");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Fee).HasColumnName("fee");
            entity.Property(e => e.IdProductType).HasColumnName("idProductType");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.IdProductTypeNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdProductType)
                .HasConstraintName("FK__Products__idProd__44A0E595");
        });

        modelBuilder.Entity<UserTypes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User typ__3213E83F78D0411F");

            entity.ToTable("User types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3213E83F9AEF4A14");

            entity.ToTable(tb => tb.HasTrigger("trg_ValidatePhoneNumber"));

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BirthDate)
                .HasColumnType("datetime")
                .HasColumnName("birthDate");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("creationDate");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("firstName");
            entity.Property(e => e.IdImage).HasColumnName("idImage");
            entity.Property(e => e.IdUserType).HasColumnName("idUserType");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("lastName");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(255)
                .HasColumnName("phoneNumber");

            entity.HasOne(d => d.IdImageNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdImage)
                .HasConstraintName("FK__Users__idImage__3FDC3078");

            entity.HasOne(d => d.IdUserTypeNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdUserType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__idUserTyp__3EE80C3F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
