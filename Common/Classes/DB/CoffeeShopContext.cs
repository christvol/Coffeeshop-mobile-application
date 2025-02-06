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

    public virtual DbSet<OrderIngredients> OrderIngredients
    {
        get; set;
    }

    public virtual DbSet<OrderProducts> OrderProducts
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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=KRISTINAVIVO\\SQLEXPRESS;Database=CoffeeShop;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<AllowedIngredients>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Allowed __3213E83F3C759896");

            _ = entity.ToTable("Allowed ingredients");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.AllowedNumber).HasColumnName("allowedNumber");
            _ = entity.Property(e => e.IdIngredient).HasColumnName("idIngredient");
            _ = entity.Property(e => e.IdProduct).HasColumnName("idProduct");

            _ = entity.HasOne(d => d.IdIngredientNavigation).WithMany(p => p.AllowedIngredients)
                .HasForeignKey(d => d.IdIngredient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Allowed i__idIng__76177A41");

            _ = entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.AllowedIngredients)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Allowed i__idPro__770B9E7A");
        });

        _ = modelBuilder.Entity<CountryCodes>(entity =>
        {
            _ = entity.HasKey(e => e.CountryTicker).HasName("PK__CountryC__A0F54A7FA5675D1F");

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
            _ = entity.HasKey(e => e.Id).HasName("PK__Images__3213E83F94E98833");

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
            _ = entity.HasKey(e => e.Id).HasName("PK__Ingredie__3213E83FB0BE4738");

            _ = entity.ToTable("Ingredient images");

            _ = entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            _ = entity.Property(e => e.IdImage).HasColumnName("idImage");
            _ = entity.Property(e => e.IdIngredient).HasColumnName("idIngredient");

            _ = entity.HasOne(d => d.IdImageNavigation).WithMany(p => p.IngredientImages)
                .HasForeignKey(d => d.IdImage)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ingredien__idIma__6F6A7CB2");

            _ = entity.HasOne(d => d.IdIngredientNavigation).WithMany(p => p.IngredientImages)
                .HasForeignKey(d => d.IdIngredient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Ingredien__idIng__6E765879");
        });

        _ = modelBuilder.Entity<IngredientTypes>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Ingredie__3213E83F9ADE2438");

            _ = entity.ToTable("Ingredient types");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        _ = modelBuilder.Entity<Ingredients>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Ingredie__3213E83F92FB85BA");

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
                .HasConstraintName("FK__Ingredien__idIng__6D823440");
        });

        _ = modelBuilder.Entity<OrderIngredients>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Order in__3213E83F09F30BEC");

            _ = entity.ToTable("Order ingredients");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.IdIngredient).HasColumnName("idIngredient");
            _ = entity.Property(e => e.IdOrder).HasColumnName("idOrder");
            _ = entity.Property(e => e.Number).HasColumnName("number");

            _ = entity.HasOne(d => d.IdIngredientNavigation).WithMany(p => p.OrderIngredients)
                .HasForeignKey(d => d.IdIngredient)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order ing__idIng__733B0D96");

            _ = entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.OrderIngredients)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order ing__idOrd__7246E95D");
        });

        _ = modelBuilder.Entity<OrderProducts>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Order pr__3213E83F70BC6228");

            _ = entity.ToTable("Order products");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.IdOrder).HasColumnName("idOrder");
            _ = entity.Property(e => e.IdProduct).HasColumnName("idProduct");
            _ = entity.Property(e => e.Number).HasColumnName("number");

            _ = entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order pro__idOrd__742F31CF");

            _ = entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.OrderProducts)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order pro__idPro__75235608");
        });

        _ = modelBuilder.Entity<Orders>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Orders__3213E83F76C75460");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("creationDate");
            _ = entity.Property(e => e.IdCustomer).HasColumnName("idCustomer");
            _ = entity.Property(e => e.IdEmployee).HasColumnName("idEmployee");

            _ = entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.OrdersIdCustomerNavigation)
                .HasForeignKey(d => d.IdCustomer)
                .HasConstraintName("FK__Orders__idCustom__6AA5C795");

            _ = entity.HasOne(d => d.IdEmployeeNavigation).WithMany(p => p.OrdersIdEmployeeNavigation)
                .HasForeignKey(d => d.IdEmployee)
                .HasConstraintName("FK__Orders__idEmploy__6B99EBCE");
        });

        _ = modelBuilder.Entity<ProductImages>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Product __3213E83F437BF2F7");

            _ = entity.ToTable("Product images");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.IdImage).HasColumnName("idImage");
            _ = entity.Property(e => e.IdProduct).HasColumnName("idProduct");

            _ = entity.HasOne(d => d.IdImageNavigation).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.IdImage)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product i__idIma__7152C524");

            _ = entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Product i__idPro__705EA0EB");
        });

        _ = modelBuilder.Entity<ProductTypes>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Product __3213E83F7078F0D8");

            _ = entity.ToTable("Product types");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        _ = modelBuilder.Entity<Products>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Products__3213E83F3DBF203E");

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
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__idProd__6C8E1007");
        });

        _ = modelBuilder.Entity<UserTypes>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__User typ__3213E83FC742DF74");

            _ = entity.ToTable("User types");

            _ = entity.Property(e => e.Id).HasColumnName("id");
            _ = entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");
        });

        _ = modelBuilder.Entity<Users>(entity =>
        {
            _ = entity.HasKey(e => e.Id).HasName("PK__Users__3213E83F4D8D7B00");

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
                .HasConstraintName("FK__Users__idImage__69B1A35C");

            _ = entity.HasOne(d => d.IdUserTypeNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdUserType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__idUserTyp__68BD7F23");
        });

        this.OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
