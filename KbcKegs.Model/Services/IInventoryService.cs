namespace KbcKegs.Model.Services
{
    public interface IInventoryService
    {
        Asset FindAssetById(int id);
        Asset CreateAsset(string serialNumber, AssetState state, string description);

        void HandleEvent(DeliveryEvent deliveryEvent);
        void HandleEvent(CollectionEvent collectionEvent);
        void HandleEvent(CleaningEvent cleaningEvent);
    }
}