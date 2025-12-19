using FPT_Booking_BE.Models;
using FPT_Booking_BE.DTOs;

namespace FPT_Booking_BE.Services
{
    public interface IAssetService
    {
        // Facility Asset operations
        Task<IEnumerable<FacilityAsset>> GetAssetsByFacilityAsync(int facilityId);
        Task<bool> UpdateAssetConditionAsync(int id, string condition, int? quantity);
        Task<(bool success, string message, FacilityAsset? asset)> CreateFacilityAssetAsync(FacilityAssetCreateRequest request);
        Task<bool> UpdateQuantityAsync(int id, int quantity);

        // Asset operations
        Task<IEnumerable<AssetDto>> GetAllAssetsAsync();
        Task<AssetDto?> GetAssetByIdAsync(int assetId);
        Task<(bool success, string message, AssetDto? asset)> CreateAssetAsync(AssetCreateRequest request);
        Task<(bool success, string message)> DeleteAssetAsync(int assetId);
    }
}