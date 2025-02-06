namespace Common.Classes.DB;

public partial class ProductImages
{
    public int Id
    {
        get; set;
    }

    public int IdProduct
    {
        get; set;
    }

    public int IdImage
    {
        get; set;
    }

    public virtual Images IdImageNavigation { get; set; } = null!;

    public virtual Products IdProductNavigation { get; set; } = null!;
}
