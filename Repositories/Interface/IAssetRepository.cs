using FPT_Booking_BE.Models;

namespace FPT_Booking_BE.Repositories
{
    public interface IAssetRepository
    {
        Task<IEnumerable<Asset>> GetAllAssetsAsync();
        Task<Asset?> GetAssetByIdAsync(int assetId);
        Task<Asset> CreateAssetAsync(Asset asset);
        Task<bool> DeleteAssetAsync(int assetId);
        Task<bool> AssetExistsAsync(int assetId);
        Task<bool> AssetNameExistsAsync(string assetName);
    }
}
