using KbcKegs.Model;

namespace KbcKegs.Models.Api
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string SourceId { get; set; }
        public string Name { get; set; }
    }

    public static class CustomerViewModelExtensions
    {
        static public CustomerViewModel ToViewModel(this Customer customer)
        {
            return new CustomerViewModel
            {
                Id = customer.Id,
                SourceId = customer.SourceId,
                Name = customer.Name,
            };
        }

        static public Customer ToNewDb(this CustomerViewModel vm)
        {
            return vm.UpdateDb(new Customer());
        }

        static public Customer UpdateDb(this CustomerViewModel vm, Customer customer)
        {
            //customer.Id = vm.Id;
            customer.Name = vm.Name;
            customer.SourceId = vm.SourceId;
            return customer;
        }
    }
}