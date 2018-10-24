using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChinookApi.Models
{
  public class Invoice
  {

    public int Id { get; set;}
    public DateTime InvoiceDate { get; set;}
    public string BillingAddress {get; set;}
    public string BillingCity {get; set;}
    public string BillingState {get; set;}
    public string BillingCountry {get; set;}
    public string BillingPostalCode {get; set;}
    public decimal Total {get; set;}
    public string Agent_Name { get; set; }
    public string Customer_Name { get; set; }
  }
}
