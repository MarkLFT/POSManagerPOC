using NubeSync.Core;
using System.ComponentModel.DataAnnotations;

namespace API.Invoices.Tests.Models;
public class Invoice : NubeTable
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
