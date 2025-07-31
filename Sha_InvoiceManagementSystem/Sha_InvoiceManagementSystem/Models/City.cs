using System;
using System.Collections.Generic;

namespace Sha_InvoiceManagementSystem.Models;

public partial class City
{
    public int Id { get; set; }

    public string CityName { get; set; } = null!;

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();
}
