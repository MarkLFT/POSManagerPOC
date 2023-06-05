using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Invoices.Tests.Models;

public class InvoiceItemEF 
{
    public Guid InvoiceItemId { get; set; }
    
    public Guid InvoiceId { get; set; }
    
    [ForeignKey("InvoiceId")]
    public virtual InvoiceEF? Invoice { get; set; }
    
    public int LineNumber { get; set; }
    
    public int? ProductId { get; set; }
    
    [Required]
    public required string Description { get; set; }
    
    public decimal Price { get; set; }
    
    public int Quantity { get; set; }

}
