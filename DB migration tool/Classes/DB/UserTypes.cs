namespace Common.Classes.DB;

public partial class UserTypes
{
    public int Id
    {
        get; set;
    }

    public string Title { get; set; } = null!;

    public virtual ICollection<Users> Users { get; set; } = new List<Users>();
}
