using NubeSync.Server.Data;
using System.ComponentModel.DataAnnotations;

namespace POSManager.API.Invoices.Models;

public class InvoiceItem : NubeServerTable
{
    public Guid InvoiceItemId { get; set; }
    public Guid InvoiceId { get; set; }
    public Invoice? Invoice { get; set; }
    public int  LineNumber { get; set; }
    public int? ProductId { get; set; }
    [Required]
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

}
