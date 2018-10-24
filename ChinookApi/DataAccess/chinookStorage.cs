using ChinookApi.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ChinookApi.DataAccess
{
  public class ChinookStorage
  {

    private const string conString = "Server=(local);Database=Chinook;Trusted_Connection=True;";

    public List<Invoice> GetbyId(int id)
    {
      using (var connection = new SqlConnection(conString))
      {
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"select Agent_Name = e.FirstName + ' ' + e.LastName, Customer_Name = c.FirstName + ' ' + c.LastName, *
                                from Invoice as i, Customer as c, Employee as e
                                where i.CustomerId = c.CustomerId
                                and e.EmployeeId = c.SupportRepId
                                and c.SupportRepId = @id
                                order by EmployeeId";

        command.Parameters.AddWithValue("@id", id);

        var reader = command.ExecuteReader();
        var invoiceHolder = new List<Invoice>();
        while (reader.Read())
        {
          var invoice = new Invoice()
          {
            Id = (int)reader["InvoiceId"],
            BillingAddress = reader["BillingAddress"].ToString(),
            BillingCity = reader["BillingCity"].ToString(),
            BillingCountry = reader["BillingCountry"].ToString(),
            BillingPostalCode = reader["BillingPostalCode"].ToString(),
            BillingState = reader["BillingState"].ToString(),
            InvoiceDate = DateTime.Parse(reader["InvoiceDate"].ToString()),
            Agent_Name = reader["Agent_Name"].ToString(),
            Customer_Name = reader["Customer_Name"].ToString(),
            Total = (decimal)reader["Total"],
          };
          invoiceHolder.Add(invoice);
        }
        return invoiceHolder;

      };

    }

  }
}
