using System;
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
using CodeLeapChallengeAPI_06022025.Data.Dto;

namespace CodeLeapChallengeAPI_06022025.Controllers
{
    /// <summary>
    /// List api for product
    /// </summary>
    [ApiController]
    [Route("product")]
    public class Productc : BaseAPIController
    {
        private readonly CodeDBContext _context;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
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
            ResponseDto<object> r = new ResponseDto<object>();
            if (ModelState.IsValid)
            {
                var prod = await _context.Products.FirstOrDefaultAsync(m => m.Name == product.Name); // Can do with base64 pass but just so little bit lazzy
                if (prod != null)
                {
                    r.RespnseStatus.StatusCode = StatusCodes.Status409Conflict;
                    r.RespnseStatus.ResponseMessage = "Product name already exited";
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                r.RespnseStatus.ResponseMessage = $"Success create new product Id: {product.Id}";
                return GetRes(r);
            }
            r.RespnseStatus.StatusCode = StatusCodes.Status422UnprocessableEntity;
            r.RespnseStatus.ResponseMessage = "Invalid Data";
            return GetRes(r);
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
            ResponseDto<object> r = new ResponseDto<object>();
            if (id != product.Id)
            {
                r.RespnseStatus.StatusCode = StatusCodes.Status400BadRequest;
                r.RespnseStatus.ResponseMessage = "Invalid Data";
                return GetRes(r);
            }

            if (ModelState.IsValid)
            {
                var prod = await _context.Products.FirstOrDefaultAsync(m => m.Name == product.Name); // Can do with base64 pass but just so little bit lazzy
                if (prod == null)
                {
                    r.RespnseStatus.StatusCode = StatusCodes.Status404NotFound;
                    r.RespnseStatus.ResponseMessage = "Can not found product";
                    return GetRes(r);
                }
                _context.Update(product);
                await _context.SaveChangesAsync();
                r.RespnseStatus.ResponseMessage = $"Success update product Id: {product.Id}";
                return GetRes(r);
            }
            r.RespnseStatus.StatusCode = StatusCodes.Status422UnprocessableEntity;
            r.RespnseStatus.ResponseMessage = "Invalid Data";
            return GetRes(r);
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
            ResponseDto<object> r = new ResponseDto<object>();
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                r.RespnseStatus.StatusCode = StatusCodes.Status404NotFound;
                r.RespnseStatus.ResponseMessage = "Can not found product";
                return GetRes(r);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            r.RespnseStatus.ResponseMessage = $"Success remove product Id: {product.Id}";
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
            ResponseDto<object> r = new ResponseDto<object>();
            if (String.IsNullOrEmpty(name) == null)
            {
                r.RespnseStatus.StatusCode = StatusCodes.Status404NotFound;
                r.RespnseStatus.ResponseMessage = "Can not found product";
                return GetRes(r);
            }

            var listproducts = await _context.Products.Where(m => m.Name.ToLower().Contains(name.ToLower())).ToListAsync();
            r.ResponseData = listproducts;
            return GetRes(r);
        }
        [NonAction]
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
