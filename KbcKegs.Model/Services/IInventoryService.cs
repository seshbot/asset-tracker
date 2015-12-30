namespace KbcKegs.Model.Services
{
    public interface IInventoryService
    {
        AssetType FindAssetTypeByDescription(string description);
        Asset FindAssetById(int id);
        Asset CreateAsset(string serialNumber, string description, AssetState state = AssetState.Unspecified);
        Asset MergeAsset(int? id, string serialNumber, string description, AssetState state = AssetState.Unspecified);

        void HandleEvent(DeliveryEvent deliveryEvent);
        void HandleEvent(CollectionEvent collectionEvent);
        void HandleEvent(CleaningEvent cleaningEvent);
    }
}