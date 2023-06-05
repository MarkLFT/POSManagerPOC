using NubeSync.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Invoices.Tests.Models;

public class InvoiceItem : NubeTable
{
    public Guid InvoiceItemId { get; set; }

    public Guid InvoiceId { get; set; }

    [ForeignKey("InvoiceId")]
    public virtual Invoice? Invoice { get; set; }

    public int LineNumber { get; set; }

    public int? ProductId { get; set; }

    [Required]
    public required string Description { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

}
