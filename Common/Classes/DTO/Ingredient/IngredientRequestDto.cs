using System.ComponentModel.DataAnnotations;

namespace Common.Classes.DTO
{
    public class IngredientRequestDto
    {
        public int Id
        {
            get; set;
        }

        public int? IdIngredientType
        {
            get; set;
        }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(255, ErrorMessage = "Title cannot be longer than 255 characters.")]
        public string Title { get; set; } = null!;

        public string? Description
        {
            get; set;
        }

        [Required(ErrorMessage = "Fee is required.")]
        public float Fee
        {
            get; set;
        }
    }
}
