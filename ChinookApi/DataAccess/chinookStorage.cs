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

    public List<InvoiceAll> GetAllInvoices()
    {
      using (var connection = new SqlConnection(conString))
      {
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"select Customer_Name = c.FirstName + ' ' + c.LastName, i.Total, c.Country, Employee_Name = e.FirstName + ' ' + e.LastName
                                from Invoice as i, Customer as c, Employee as e
                                where i.CustomerId = c.CustomerId
                                and e.EmployeeId = c.SupportRepId";

        var reader = command.ExecuteReader();
        var invoiceList = new List<InvoiceAll>();

        while (reader.Read())
        {
          var invoice = new InvoiceAll()
          {
            Customer_Name = reader["Customer_Name"].ToString(),
            Total = (decimal)reader["Total"],
            Country = reader["Country"].ToString(),
            Employee_Name = reader["Employee_Name"].ToString(),
          };

          invoiceList.Add(invoice);
        }

        return invoiceList;
      }
    }

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

    public object CountInvoiceLine(int id)
    {
      using (var connection = new SqlConnection(conString))
      {
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"Select count(InvoiceLineId) as Line_Items
                                from InvoiceLine join Invoice
	                              on Invoice.InvoiceId = InvoiceLine.InvoiceId and Invoice.InvoiceId = @id";
        command.Parameters.AddWithValue("@id", id);

        var result = command.ExecuteScalar();
        return result;
      }
    }

    public bool AddInvoice(Invoice invoice)
    {
      using (var connection = new SqlConnection(conString))
      {
        connection.Open();
        var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO [dbo].[Invoice]([CustomerId],[InvoiceDate],[BillingAddress]
                                ,[BillingCity],[BillingState],[BillingCountry],[BillingPostalCode],[Total])
                                Values (@CustomerId,@InvoiceDate,@BillingAddress,@BillingCity,@BillingState
                                ,@BillingCountry,@BillingPostalCode,@Total)";
        command.Parameters.AddWithValue("@CustomerId", invoice.CustomerId);
        command.Parameters.AddWithValue("@InvoiceDate", invoice.InvoiceDate);
        command.Parameters.AddWithValue("@BillingAddress", invoice.BillingAddress);
        command.Parameters.AddWithValue("@BillingCity", invoice.BillingCity);
        command.Parameters.AddWithValue("@BillingState", invoice.BillingState);
        command.Parameters.AddWithValue("@BillingCountry", invoice.BillingCountry);
        command.Parameters.AddWithValue("@BillingPostalCode", invoice.BillingPostalCode);
        command.Parameters.AddWithValue("@Total", invoice.Total);

        var result = command.ExecuteNonQuery();
        return result == 1;

      }
    }

  }
}
