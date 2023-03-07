using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<DeliveryMethod> _dmRepo;
        private readonly IGenericRepository<Order> _orderRepo;
        public OrderService(
            IGenericRepository<Order> orderRepo,
            IGenericRepository<DeliveryMethod> dmRepo,
            IGenericRepository<Product> productRepo,
            IBasketRepository basketRepo
        )
        {
            _orderRepo = orderRepo;
            _dmRepo = dmRepo;
            _productRepo = productRepo;
            _basketRepo = basketRepo;
        }

        public async Task<Order> CreateOrderAsync(
            string buyerEmail,
            int deliveryMethodId,
            string basketId,
            Address shippingAddress)
        {
            var basket = await _basketRepo.GetBasketAsync(basketId);

            var items = new List<OrderItem>();
            foreach (var item in basket!.Items)
            {
                var productItem = await _productRepo.GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem!.Id, productItem.Name, productItem.PictureUrl);
                var orderitem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderitem);
            }

            var deliveryMethod = await _dmRepo.GetByIdAsync(deliveryMethodId);

            var subTotal = items.Sum(item => item.Price * item.Quantity);

            var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod!, subTotal);

            // TODO: save to db

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}