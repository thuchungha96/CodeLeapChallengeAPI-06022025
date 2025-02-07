﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CodeLeapChallengeAPI_06022025.Data.Class;
using CodeLeapChallengeAPI_06022025.Data.Context;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace CodeLeapChallengeAPI_06022025.Controllers
{
    /// <summary>
    /// List api for product
    /// </summary>
    [ApiController]
    [Route("product")]
    public class Productc : Controller
    {
        private readonly CodeDBContext _context;

        public Productc(CodeDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create new product and save into database
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImageURL,Note")] Product product)
        {
            if (ModelState.IsValid)
            {
                var prod = await _context.Products.FirstOrDefaultAsync(m => m.Name == product.Name); // Can do with base64 pass but just so little bit lazzy
                if (prod != null)
                    return Ok("Product already exists");

                _context.Add(product);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return Ok();
        }
        /// <summary>
        /// Edit information of exited product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost("edit")]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageURL,Note")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var prod = await _context.Products.FirstOrDefaultAsync(m => m.Name == product.Name); // Can do with base64 pass but just so little bit lazzy
                    if (prod == null)
                        return NotFound();
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Ok();
        }
        // POST: Product/Delete/5
        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("delete")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Search list of product contain string value in name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("search")]
        public async Task<IActionResult> Details(string name)
        {
            if (String.IsNullOrEmpty(name) == null)
                return NotFound();

            var listproducts = await _context.Products.Where(m => m.Name.ToLower().Contains(name.ToLower())).ToListAsync();
            if (listproducts == null)
            {
                return NotFound();
            }
            return Ok(JsonConvert.SerializeObject(listproducts).ToString());
        }
        [NonAction]
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
