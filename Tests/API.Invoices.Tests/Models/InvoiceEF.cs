using System.ComponentModel.DataAnnotations;

namespace API.Invoices.Tests.Models;
public class InvoiceEF //: NubeTable
{
    public Guid InvoiceId { get; set; }
    [Required]
    public required string InvoiceNo { get; set; }
    [Required]
    public required string GuestName { get; set; }
    public DateTime IssuedDate { get; set; }
    public int? TableId { get; set; }
    public List<InvoiceItemEF> InvoiceItems { get; set; } = new List<InvoiceItemEF>();

}
