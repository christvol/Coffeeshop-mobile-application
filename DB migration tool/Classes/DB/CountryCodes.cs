using System;
using System.Collections.Generic;

namespace Common.Classes.DB;

public partial class CountryCodes
{
    public int Id { get; set; }

    public string CountryTicker { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    public string CountryName { get; set; } = null!;
}
