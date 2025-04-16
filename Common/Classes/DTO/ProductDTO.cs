namespace Common.Classes.DTO;

public class ProductDTO
{
    #region Свойства
    public int Id
    {
        get; set;
    }

    public string Title { get; set; } = null!;

    public string? Description
    {
        get; set;
    }

    public float Fee
    {
        get; set;
    }

    public int IdProductType
    {
        get; set;
    }

    public List<string> ProductImages { get; set; } = new();

    public List<int> ProductImageIds { get; set; } = new(); // id изображений, а не url

    #endregion
}
