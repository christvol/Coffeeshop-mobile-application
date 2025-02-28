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

    public virtual DbSet<AllowedIngredients> AllowedIngredients
    {
        get; set;
    }

    public virtual DbSet<CountryCodes> CountryCodes
    {
        get; set;
    }

    public virtual DbSet<Images> Images
    {
        get; set;
    }

    public virtual DbSet<IngredientImages> IngredientImages
    {
        get; set;
    }

    public virtual DbSet<IngredientTypes> IngredientTypes
    {
        get; set;
    }

    public virtual DbSet<Ingredients> Ingredients
    {
        get; set;
    }

    public virtual DbSet<IngredientsView> IngredientsView
    {
        get; set;
    }

    public virtual DbSet<OrderItemIngredients> OrderItemIngredients
    {
        get; set;
    }

    public virtual DbSet<OrderItems> OrderItems
    {
        get; set;
    }

    public virtual DbSet<OrderProducts> OrderProducts
    {
        get; set;
    }

    public virtual DbSet<OrderStatuses> OrderStatuses
    {
        get; set;
    }

    public virtual DbSet<Orders> Orders
    {
        get; set;
    }

    public virtual DbSet<ProductImages> ProductImages
    {
        get; set;
    }

    public virtual DbSet<ProductTypes> ProductTypes
    {
        get; set;
    }

    public virtual DbSet<Products> Products
    {
        get; set;
    }

    public virtual DbSet<UserTypes> UserTypes
    {
        get; set;
    }

    public virtual DbSet<Users> Users
    {
        get; set;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _ = optionsBuilder.UseSqlServer("Server=LAPTOP-BBFM8MMD\\SQLEXPRESS;Database=CoffeeShop;Trusted_Connection=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<AllowedIngredients>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Allowed __3213E83FEC3DC0E2");

            _ = entity.ToTable("Allowed ingredients");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.AllowedNumber).HasColumnName("allowedNumber");
            _ = entity.Property(e => e.IdIngredient).HasColumnName("idIngredient");
            _ = entity.Property(e => e.IdProduct).HasColumnName("idProduct");

            _ = entity.HasOne(d => d.IdIngredientNavigation).WithMany(p => p.AllowedIngredients)
                .HasForeignKey(d => d.IdIngredient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Allowed i__idIng__3D89D3B2");

            _ = entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.AllowedIngredients)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("FK__Allowed i__idPro__3E7DF7EB");
        });

        _ = modelBuilder.Entity<CountryCodes>(entity =>
        {
            _ = entity.HasKey(e => e.CountryTicker).HasName("PK__CountryC__A0F54A7F89B7CCF5");

            _ = entity.Property(e => e.CountryTicker)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("countryTicker");
            _ = entity.Property(e => e.CountryCode)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("countryCode");
            _ = entity.Property(e => e.CountryName)
                .HasMaxLength(100)
                .HasColumnName("countryName");
            _ = entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
        });

        _ = modelBuilder.Entity<Images>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Images__3213E83F9292B63E");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.Data).HasColumnName("data");
            _ = entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            _ = entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
            _ = entity.Property(e => e.Url)
                .HasMaxLength(1024)
                .HasColumnName("url");
        });

        _ = modelBuilder.Entity<IngredientImages>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Ingredie__3213E83F5490B8A4");

            _ = entity.ToTable("Ingredient images");

            _ = entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            _ = entity.Property(e => e.IdImage).HasColumnName("idImage");
            _ = entity.Property(e => e.IdIngredient).HasColumnName("idIngredient");

            _ = entity.HasOne(d => d.IdImageNavigation).WithMany(p => p.IngredientImages)
                .HasForeignKey(d => d.IdImage)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ingredien__idIma__3AAD6707");

            _ = entity.HasOne(d => d.IdIngredientNavigation).WithMany(p => p.IngredientImages)
                .HasForeignKey(d => d.IdIngredient)
                .HasConstraintName("FK__Ingredien__idIng__39B942CE");
        });

        _ = modelBuilder.Entity<IngredientTypes>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Ingredie__3213E83FB322F621");

            _ = entity.ToTable("Ingredient types");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        _ = modelBuilder.Entity<Ingredients>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Ingredie__3213E83FE3EB0884");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            _ = entity.Property(e => e.Fee).HasColumnName("fee");
            _ = entity.Property(e => e.IdIngredientType).HasColumnName("idIngredientType");
            _ = entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            _ = entity.HasOne(d => d.IdIngredientTypeNavigation).WithMany(p => p.Ingredients)
                .HasForeignKey(d => d.IdIngredientType)
                .HasConstraintName("FK__Ingredien__idIng__38C51E95");
        });

        _ = modelBuilder.Entity<IngredientsView>(entity =>
        {
            _ = entity
                .HasNoKey()
                .ToView("IngredientsView");

            _ = entity.Property(e => e.IngredientDescription).HasMaxLength(255);
            _ = entity.Property(e => e.IngredientTitle).HasMaxLength(255);
            _ = entity.Property(e => e.IngredientTypeTitle).HasMaxLength(255);
        });

        _ = modelBuilder.Entity<OrderItemIngredients>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Order it__3213E83F818E6A5E");

            _ = entity.ToTable("Order item ingredients");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.Amount).HasColumnName("amount");
            _ = entity.Property(e => e.IdIngredient).HasColumnName("idIngredient");
            _ = entity.Property(e => e.IdOrderProduct).HasColumnName("idOrderProduct");

            _ = entity.HasOne(d => d.IdIngredientNavigation).WithMany(p => p.OrderItemIngredients)
                .HasForeignKey(d => d.IdIngredient)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Order ite__idIng__4342AD08");

            _ = entity.HasOne(d => d.IdOrderProductNavigation).WithMany(p => p.OrderItemIngredients)
                .HasForeignKey(d => d.IdOrderProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order ite__idOrd__424E88CF");
        });

        _ = modelBuilder.Entity<OrderItems>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Order it__3213E83F517311B4");

            _ = entity.ToTable("Order items");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.IdOrder).HasColumnName("idOrder");
            _ = entity.Property(e => e.IdOrderProduct).HasColumnName("idOrderProduct");

            _ = entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order ite__idOrd__3F721C24");

            _ = entity.HasOne(d => d.IdOrderProductNavigation).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.IdOrderProduct)
                .HasConstraintName("FK__Order ite__idOrd__4066405D");
        });

        _ = modelBuilder.Entity<OrderProducts>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Order pr__3213E83F05BEC87A");

            _ = entity.ToTable("Order products");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.IdProduct).HasColumnName("idProduct");
            _ = entity.Property(e => e.Total).HasColumnName("total");

            _ = entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Order pro__idPro__415A6496");
        });

        _ = modelBuilder.Entity<OrderStatuses>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__OrderSta__3213E83F69513550");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");
        });

        _ = modelBuilder.Entity<Orders>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Orders__3213E83FB39AAF17");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("creationDate");
            _ = entity.Property(e => e.IdCustomer).HasColumnName("idCustomer");
            _ = entity.Property(e => e.IdEmployee).HasColumnName("idEmployee");
            _ = entity.Property(e => e.IdStatus).HasColumnName("idStatus");

            _ = entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.OrdersIdCustomerNavigation)
                .HasForeignKey(d => d.IdCustomer)
                .HasConstraintName("FK__Orders__idCustom__34F48DB1");

            _ = entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.OrdersIdEmployeeNavigation)
                .HasForeignKey(d => d.IdEmployee)
                .HasConstraintName("FK__Orders__idEmploy__35E8B1EA");

            _ = entity.HasOne(d => d.IdStatusNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__idStatus__36DCD623");
        });

        _ = modelBuilder.Entity<ProductImages>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Product __3213E83F66167E2A");

            _ = entity.ToTable("Product images");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.IdImage).HasColumnName("idImage");
            _ = entity.Property(e => e.IdProduct).HasColumnName("idProduct");

            _ = entity.HasOne(d => d.IdImageNavigation).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.IdImage)
                .HasConstraintName("FK__Product i__idIma__3C95AF79");

            _ = entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("FK__Product i__idPro__3BA18B40");
        });

        _ = modelBuilder.Entity<ProductTypes>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Product __3213E83FBE05A630");

            _ = entity.ToTable("Product types");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        _ = modelBuilder.Entity<Products>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Products__3213E83F23B0CF43");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            _ = entity.Property(e => e.Fee).HasColumnName("fee");
            _ = entity.Property(e => e.IdProductType).HasColumnName("idProductType");
            _ = entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            _ = entity.HasOne(d => d.IdProductTypeNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdProductType)
                .HasConstraintName("FK__Products__idProd__37D0FA5C");
        });

        _ = modelBuilder.Entity<UserTypes>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__User typ__3213E83F16EBBDC9");

            _ = entity.ToTable("User types");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        _ = modelBuilder.Entity<Users>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Users__3213E83FE486FC66");

            _ = entity.ToTable(tb => tb.HasTrigger("trg_ValidatePhoneNumber"));

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.BirthDate)
                .HasColumnType("datetime")
                .HasColumnName("birthDate");
            _ = entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("creationDate");
            _ = entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            _ = entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("firstName");
            _ = entity.Property(e => e.IdImage).HasColumnName("idImage");
            _ = entity.Property(e => e.IdUserType).HasColumnName("idUserType");
            _ = entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("lastName");
            _ = entity.Property(e => e.PhoneNumber)
                .HasMaxLength(255)
                .HasColumnName("phoneNumber");

            _ = entity.HasOne(d => d.IdImageNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdImage)
                .HasConstraintName("FK__Users__idImage__34006978");

            _ = entity.HasOne(d => d.IdUserTypeNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdUserType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__idUserTyp__330C453F");
        });

        this.OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
