// DTO (Data Transfer Object) version of Products class to avoid navigation property recursion
namespace REST_API_SERVER.DTOs;

public class ProductDTO
{
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

    public string ProductTypeTitle { get; set; } = null!; // Added to include basic information about ProductType

    public List<string> ProductImages { get; set; } = new(); // A list of image URLs or identifiers
}
