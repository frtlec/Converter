using API.Databases.Mongo.DataModels;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FollowedProductsController : ControllerBase
    {
        private readonly FollowedProductService _service;
        public FollowedProductsController(FollowedProductService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try {
                return Ok(await _service.GetAll());
            }
            catch (Exception ex) {
                return BadRequest(ex.Message); 
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]FollowedProductAddDto item)
        {
            await _service.Add(item);
            return Ok(true);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Remove(string id)
        {
            await _service.Remove(id);
            return Ok(true);
        }
        [HttpGet]
        public async Task<IActionResult> RemoveByProductIdAndSourceSite(int productId,SourceSite sourceSite)
        {
            await _service.RemoveByProductIdAndSource(productId,sourceSite);
            return Ok(true);
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] FollowedProductUpdate item)
        {
            await _service.Update(item);
            return Ok(true);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveAndAdd([FromBody] List<FollowedProductAddDto> items)
        {
            await _service.RemoveAndAdd(items.Select(f=>new FollowedProduct
            {
                Barcode = f.Barcode,
                SourceSite = SourceSite.TAHTAKALE,
                ProductId = f.ProductId
            }).ToList());
            return Ok(true);
        }
    }
}
