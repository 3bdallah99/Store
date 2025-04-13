using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManger serviceManger) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllProducts ()
        {
            var result = await serviceManger.productService.GetAllProductsAsync();
            if (result == null) return BadRequest();
            return  Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await serviceManger.productService.GetProductbyIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await serviceManger.productService.GetAllBrandsAsync();
            if (result == null) return BadRequest();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await serviceManger.productService.GetAllTypesAsync();
            if (result == null) return BadRequest();
            return Ok(result);
        }


    }
}
