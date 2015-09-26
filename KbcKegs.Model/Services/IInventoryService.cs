namespace KbcKegs.Model.Services
{
    public interface IInventoryService
    {
        void HandleEvent(DeliveryEvent deliveryEvent);
        void HandleEvent(CollectionEvent collectionEvent);
        void HandleEvent(CleaningEvent cleaningEvent);
    }
}