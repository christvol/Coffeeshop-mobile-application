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

    public virtual DbSet<OrderItemIngredients> OrderItemIngredients { get; set; }

    public virtual DbSet<OrderItems> OrderItems { get; set; }

    public virtual DbSet<OrderProducts> OrderProducts { get; set; }

    public virtual DbSet<Orders> Orders { get; set; }

    public virtual DbSet<ProductImages> ProductImages { get; set; }

    public virtual DbSet<ProductTypes> ProductTypes { get; set; }

    public virtual DbSet<Products> Products { get; set; }

    public virtual DbSet<UserTypes> UserTypes { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=KRISTINAVIVO\\SQLEXPRESS;Database=CoffeeShop;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AllowedIngredients>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Allowed __3213E83F21AB76A6");

            entity.ToTable("Allowed ingredients");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AllowedNumber).HasColumnName("allowedNumber");
            entity.Property(e => e.IdIngredient).HasColumnName("idIngredient");
            entity.Property(e => e.IdProduct).HasColumnName("idProduct");

            entity.HasOne(d => d.IdIngredientNavigation).WithMany(p => p.AllowedIngredients)
                .HasForeignKey(d => d.IdIngredient)
                .HasConstraintName("FK__Allowed i__idIng__62265B84");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.AllowedIngredients)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("FK__Allowed i__idPro__631A7FBD");
        });

        modelBuilder.Entity<CountryCodes>(entity =>
        {
            entity.HasKey(e => e.CountryTicker).HasName("PK__CountryC__A0F54A7F8B91DA9A");

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
            entity.HasKey(e => e.Id).HasName("PK__Images__3213E83F3CAE4579");

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
            entity.HasKey(e => e.Id).HasName("PK__Ingredie__3213E83F59862F4E");

            entity.ToTable("Ingredient images");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.IdImage).HasColumnName("idImage");
            entity.Property(e => e.IdIngredient).HasColumnName("idIngredient");

            entity.HasOne(d => d.IdImageNavigation).WithMany(p => p.IngredientImages)
                .HasForeignKey(d => d.IdImage)
                .HasConstraintName("FK__Ingredien__idIma__5F49EED9");

            entity.HasOne(d => d.IdIngredientNavigation).WithMany(p => p.IngredientImages)
                .HasForeignKey(d => d.IdIngredient)
                .HasConstraintName("FK__Ingredien__idIng__5E55CAA0");
        });

        modelBuilder.Entity<IngredientTypes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ingredie__3213E83FF758752A");

            entity.ToTable("Ingredient types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Ingredients>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ingredie__3213E83F2D720FC0");

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
                .HasConstraintName("FK__Ingredien__idIng__5D61A667");
        });

        modelBuilder.Entity<OrderItemIngredients>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order it__3213E83F04D5A162");

            entity.ToTable("Order item ingredients");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.IdIngredient).HasColumnName("idIngredient");
            entity.Property(e => e.IdOrderProduct).HasColumnName("idOrderProduct");

            entity.HasOne(d => d.IdIngredientNavigation).WithMany(p => p.OrderItemIngredients)
                .HasForeignKey(d => d.IdIngredient)
                .HasConstraintName("FK__Order ite__idIng__67DF34DA");

            entity.HasOne(d => d.IdOrderProductNavigation).WithMany(p => p.OrderItemIngredients)
                .HasForeignKey(d => d.IdOrderProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order ite__idOrd__66EB10A1");
        });

        modelBuilder.Entity<OrderItems>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order it__3213E83FF418422F");

            entity.ToTable("Order items");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdOrder).HasColumnName("idOrder");
            entity.Property(e => e.IdOrderProduct).HasColumnName("idOrderProduct");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order ite__idOrd__640EA3F6");

            entity.HasOne(d => d.IdOrderProductNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.IdOrderProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order ite__idOrd__6502C82F");
        });

        modelBuilder.Entity<OrderProducts>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Order pr__3213E83FF47A2533");

            entity.ToTable("Order products");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdProduct).HasColumnName("idProduct");
            entity.Property(e => e.Total).HasColumnName("total");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("FK__Order pro__idPro__65F6EC68");
        });

        modelBuilder.Entity<Orders>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3213E83F484B1E4C");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("creationDate");
            entity.Property(e => e.IdCustomer).HasColumnName("idCustomer");
            entity.Property(e => e.IdEmployee).HasColumnName("idEmployee");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.OrdersIdCustomerNavigation)
                .HasForeignKey(d => d.IdCustomer)
                .HasConstraintName("FK__Orders__idCustom__5A8539BC");

            entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.OrdersIdEmployeeNavigation)
                .HasForeignKey(d => d.IdEmployee)
                .HasConstraintName("FK__Orders__idEmploy__5B795DF5");
        });

        modelBuilder.Entity<ProductImages>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product __3213E83F639DAD6C");

            entity.ToTable("Product images");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdImage).HasColumnName("idImage");
            entity.Property(e => e.IdProduct).HasColumnName("idProduct");

            entity.HasOne(d => d.IdImageNavigation).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.IdImage)
                .HasConstraintName("FK__Product i__idIma__6132374B");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("FK__Product i__idPro__603E1312");
        });

        modelBuilder.Entity<ProductTypes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product __3213E83F53D6A905");

            entity.ToTable("Product types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Products>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3213E83F8F7D1E0D");

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
                .HasConstraintName("FK__Products__idProd__5C6D822E");
        });

        modelBuilder.Entity<UserTypes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User typ__3213E83F9DCC0682");

            entity.ToTable("User types");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3213E83F8F4A6049");

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
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Users__idImage__59911583");

            entity.HasOne(d => d.IdUserTypeNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdUserType)
                .HasConstraintName("FK__Users__idUserTyp__589CF14A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
