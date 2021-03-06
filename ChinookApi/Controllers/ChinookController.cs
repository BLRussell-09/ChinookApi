﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChinookApi.DataAccess;
using ChinookApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ChinookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChinookController : ControllerBase
    {
        private readonly ChinookStorage _storage;

        public ChinookController(ChinookStorage config)
        {
            _storage = config;
        }

        [HttpGet("{id}")]
        public IActionResult GetInvoicesByAgent(int id)
        {
          return Ok(_storage.GetbyId(id));
        }

        [HttpGet("invoice")]
        public IActionResult GetInvoices()
        {
          return Ok(_storage.GetAllInvoices());
        }

        [HttpGet("invoice/{id}/count")]
        public IActionResult GetCount(int id)
        {
          return Ok(_storage.CountInvoiceLine(id));
        }

        [HttpPost]
        public void AddAnInvoice(Invoice invoice)
        {
          _storage.AddInvoice(invoice);
        }

        [HttpPut("{id}")]
        public void UpdateAnEmployee(Employee employee, int id)
        {
           _storage.UpdateEmployee(employee, id);
        }
    }
}