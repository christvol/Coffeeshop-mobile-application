namespace Common.Classes.DB;

public partial class Users
{
    public string FullName => $"{this.FirstName} {this.LastName}".Trim();
}
