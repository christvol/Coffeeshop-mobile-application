namespace Common.Classes.DB;

public partial class OrderIngredients
{
    public int Id
    {
        get; set;
    }

    public int IdOrder
    {
        get; set;
    }

    public int IdIngredient
    {
        get; set;
    }

    public byte Number
    {
        get; set;
    }

    public virtual Ingredients IdIngredientNavigation { get; set; } = null!;

    public virtual Orders IdOrderNavigation { get; set; } = null!;
}
