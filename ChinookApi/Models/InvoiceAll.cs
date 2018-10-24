using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChinookApi.Models
{
  public class InvoiceAll
  {
    public string Customer_Name { get; internal set; }
    public string Employee_Name { get; internal set; }
    public string Country { get; internal set; }
    public decimal Total { get; internal set; }
  }
}
