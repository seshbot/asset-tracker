using KbcKegs.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Model.Services
{
    public class ShopService : IShopService
    {
        private ICustomerRepository _customers;
        private IOrderRepository _orders;

        public ShopService(IOrderRepository orders, ICustomerRepository customers)
        {
            _orders = orders;
            _customers = customers;
        }

        public Order MergeOrder(int? id, string sourceId, string customerSourceId)
        {
            if (id.HasValue && id.Value > 0)
            {
                return FindOrderById(id.Value);
            }

            return FindOrder(sourceId, customerSourceId);
        }

        private Customer FindCustomerBySourceId(string customerSourceId)
        {
            if (string.IsNullOrEmpty(customerSourceId))
            {
                return null;
            }

            var customer = _customers.GetBySourceId(customerSourceId);

            if (null == customer)
            {
                customer = new Customer { SourceId = customerSourceId };
                _customers.Add(customer);
            }

            return customer;
        }

        public Order FindOrder(string sourceId, string customerSourceId)
        {
            if (!string.IsNullOrEmpty(sourceId))
            {
                var existingOrder = _orders.GetBySourceId(sourceId);
                if (null != existingOrder)
                {
                    if (string.IsNullOrEmpty(existingOrder.Customer?.SourceId))
                    {
                        if (!string.IsNullOrEmpty(customerSourceId))
                        {
                            existingOrder.Customer = FindCustomerBySourceId(customerSourceId);
                        }
                    }
                    return existingOrder;
                }
            }

            var order = new Order
            {
                SourceId = sourceId,
                Customer = FindCustomerBySourceId(customerSourceId)
            };
            _orders.Add(order);

            return order;
        }

        public Order FindOrderById(int orderId)
        {
            return _orders.GetById(orderId);
        }
    }
}
