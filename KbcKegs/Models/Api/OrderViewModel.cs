using KbcKegs.Model;

namespace KbcKegs.Models.Api
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string SourceId { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
    }

    public static class OrderViewModelExtensions
    {
        static public OrderViewModel ToViewModel(this Order order)
        {
            return new OrderViewModel
            {
                Id = order.Id,
                SourceId = order.SourceId,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer.Name,
            };
        }

        static public Order ToNewDb(this OrderViewModel vm)
        {
            return vm.UpdateDb(new Order());
        }

        static public Order UpdateDb(this OrderViewModel vm, Order order)
        {
            //customer.Id = vm.Id;
            order.SourceId = vm.SourceId;
            order.CustomerId = vm.CustomerId;
            return order;
        }
    }
}