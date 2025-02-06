namespace Common.Classes.DB;

public partial class OrderProducts
{
    public int Id
    {
        get; set;
    }

    public int IdOrder
    {
        get; set;
    }

    public int IdProduct
    {
        get; set;
    }

    public byte Number
    {
        get; set;
    }

    public virtual Orders IdOrderNavigation { get; set; } = null!;

    public virtual Products IdProductNavigation { get; set; } = null!;
}
