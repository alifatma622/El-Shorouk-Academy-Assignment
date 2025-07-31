using System;
using System.Collections.Generic;

namespace Sha_InvoiceManagementSystem.Models;

public partial class InvoiceDetail
{
    public long Id { get; set; }

    public long InvoiceHeaderId { get; set; }

    public string ItemName { get; set; } = null!;

    public double ItemCount { get; set; }

    public double ItemPrice { get; set; }

    public virtual InvoiceHeader InvoiceHeader { get; set; } = null!;
}
