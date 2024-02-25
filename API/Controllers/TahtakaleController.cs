using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TahtakaleController : ControllerBase
    {
        private readonly TahtakaleService tahtakaleService;
        public TahtakaleController(TahtakaleService tahtakaleService)
        {
            this.tahtakaleService = tahtakaleService;
        }
        [HttpPost]
        public async Task<IActionResult> GetAllByFilter(GetAllByFilter getAllByFilter)
        {

            var resp = await this.tahtakaleService.GetAllByFilter(getAllByFilter);
            return Ok(resp);
        }
        [HttpGet]
        public async Task<IActionResult> GetExcelExport()
        {

            //var resp = await this.tahtakaleService.GetExcelBytes();
            return Ok("");
        }

    }
}
