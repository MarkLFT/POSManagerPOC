using NubeSync.Server.Data;
using System.ComponentModel.DataAnnotations;

namespace POSManager.API.Invoices.Models;

public class Invoice : NubeServerTable
{
    public Guid InvoiceId { get; set; }
    [Required]
    public required string InvoiceNo { get; set; }
    [Required]
    public required string GuestName { get; set; }
    public DateTime IssuedDate { get; set; }
    public int? TableId { get; set; }
    public IEnumerable<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

}
