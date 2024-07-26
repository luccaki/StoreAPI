using Microsoft.AspNetCore.Mvc;
using StoreAPI.Dtos;
using StoreAPI.Services;

namespace StoreAPI.Controllers
{
    [Route("api/companies/{companyId}/stores")]
    [ApiController]
    public class StoreController : ControllerBase
    {
        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStore(long companyId, CreateUpdateStoreDto storeDto)
        {
            var store = await _storeService.CreateAsync(companyId, storeDto);
            return CreatedAtAction(nameof(GetStoreById), new { companyId, id = store.Id }, store);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStoreById(long companyId, long id)
        {
            var store = await _storeService.GetByIdAsync(companyId, id);
            return Ok(store);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStore(long companyId, long id, CreateUpdateStoreDto updateStoreDto)
        {
            await _storeService.UpdateAsync(companyId, id, updateStoreDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStore(long companyId, long id)
        {
            await _storeService.DeleteAsync(companyId, id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStores(long companyId)
        {
            var stores = await _storeService.GetAllAsync(companyId);
            return Ok(stores);
        }
    }
}