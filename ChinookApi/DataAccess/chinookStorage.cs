using ChinookApi.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ChinookApi.DataAccess
{
  public class ChinookStorage
  {

    private readonly string conString;

    public ChinookStorage(IConfiguration config)
    {
      conString = config.GetSection("ConnectionString").Value;
    }

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

    public IEnumerable<Invoice> GetbyId(int id)
    {
      using (var connection = new SqlConnection(conString))
      {
        connection.Open();

        var result = connection.Query<Invoice>(@"select Agent_Name = e.FirstName + ' ' + e.LastName, Customer_Name = c.FirstName + ' ' + c.LastName, *
                                from Invoice as i, Customer as c, Employee as e
                                where i.CustomerId = c.CustomerId
                                and e.EmployeeId = c.SupportRepId
                                and c.SupportRepId = @id
                                order by EmployeeId", new {id = id });

        return result;

      };

    }

    public object CountInvoiceLine(int id)
    {
      using (var connection = new SqlConnection(conString))
      {
        connection.Open();
        
        var result = connection.QueryFirst<object>(@"Select count(InvoiceLineId) as Line_Items
                                from InvoiceLine join Invoice
	                              on Invoice.InvoiceId = InvoiceLine.InvoiceId and Invoice.InvoiceId = @id", new {id = id});

        return result;
      }
    }

    public bool AddInvoice(Invoice invoice)
    {
      using (var connection = new SqlConnection(conString))
      {
        connection.Open();

        var result = connection.Execute(@"INSERT INTO [dbo].[Invoice]([CustomerId],[InvoiceDate],[BillingAddress],[BillingCity],[BillingState],[BillingCountry],[BillingPostalCode],[Total])
                             Values (@CustomerId,@InvoiceDate,@BillingAddress,@BillingCity,@BillingState,@BillingCountry,@BillingPostalCode,@Total)", invoice);
       
        return result == 1;
      }
    }

    public bool UpdateEmployee(Employee employee, int id)
    {
      using (var connection = new SqlConnection(conString))
      {
        connection.Open();

        var result = connection.Execute(@"UPDATE [dbo].[Employee]
                                  SET[LastName] = @LastName, [FirstName] = @FirstName
                                  WHERE Employee.EmployeeId = @id", new { id, LastName = employee.LastName, FirstName = employee.FirstName});
        //var command = connection.CreateCommand();
        //command.CommandText = @"UPDATE [dbo].[Employee]
        //                          SET[LastName] = @LastName, [FirstName] = @FirstName
        //                          WHERE Employee.EmployeeId = @id";
        //command.Parameters.AddWithValue("@LastName", employee.LastName);
        //command.Parameters.AddWithValue("@FirstName", employee.FirstName);
        //command.Parameters.AddWithValue("@id", id);

        //var result = command.ExecuteNonQuery();
        return result == 1;
      }
    }

  }
}
