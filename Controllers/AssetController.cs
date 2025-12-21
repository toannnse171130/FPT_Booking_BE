using Microsoft.AspNetCore.Mvc;
using FPT_Booking_BE.Services;
using FPT_Booking_BE.DTOs;

namespace FPT_Booking_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        /// <summary>
        /// Get all assets
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllAssets()
        {
            var assets = await _assetService.GetAllAssetsAsync();
            return Ok(assets);
        }

        /// <summary>
        /// Get asset by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssetById(int id)
        {
            var asset = await _assetService.GetAssetByIdAsync(id);
            
            if (asset == null)
            {
                return NotFound(new { message = "Không tìm thấy tài sản." });
            }

            return Ok(asset);
        }

        /// <summary>
        /// Create new asset
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateAsset([FromBody] AssetCreateRequest request)
        {
            var (success, message, asset) = await _assetService.CreateAssetAsync(request);

            if (!success)
            {
                return BadRequest(new { message });
            }

            return CreatedAtAction(nameof(GetAssetById), new { id = asset!.AssetId }, new
            {
                message,
                data = asset
            });
        }

        /// <summary>
        /// Delete asset by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsset(int id)
        {
            var (success, message) = await _assetService.DeleteAssetAsync(id);

            if (!success)
            {
                return BadRequest(new { message });
            }

            return Ok(new { message });
        }
    }
}
