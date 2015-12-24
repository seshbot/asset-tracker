using KbcKegs.Model;

namespace KbcKegs.Models.Api
{
    public class AssetViewModel
    {
        public int? Id { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }
    }

    public static class AssetViewModelExtensions
    {
        static public AssetViewModel ToViewModel(this Asset asset)
        {
            return new AssetViewModel
            {
                Id = asset.Id,
                SerialNumber = asset.SerialNumber,
                Description = asset.Description,
            };
        }

        static public Asset ToNewDb(this AssetViewModel vm, AssetState initialState = AssetState.Available)
        {
            return vm.UpdateDb(new Asset { State = initialState });
        }

        static public Asset UpdateDb(this AssetViewModel vm, Asset asset)
        {
            asset.SerialNumber = vm.SerialNumber;
            asset.Description = vm.Description;

            return asset;
        }
    }
}