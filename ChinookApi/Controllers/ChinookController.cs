﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChinookApi.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChinookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChinookController : ControllerBase
    {
        private readonly ChinookStorage _storage;

        public ChinookController()
        {
            _storage = new ChinookStorage();
        }

        [HttpGet("{id}")]
        public IActionResult GetInvoicesByAgent(int id)
        {
          return Ok(_storage.GetbyId(id));
        }

    }
}