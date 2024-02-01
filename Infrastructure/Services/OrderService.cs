using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IGenericRepository<Order> _orderRepo;
    private readonly IGenericRepository<DeliveryMethod> _deliveryRepo;
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IBasketRepository _basketRepo;

    public OrderService(
        IGenericRepository<Order> orderRepo,
        IGenericRepository<DeliveryMethod> deliveryRepo,
        IGenericRepository<Product> productRepo,
        IBasketRepository basketRepo
    )
    {
        _orderRepo = orderRepo;
        _deliveryRepo = deliveryRepo;
        _productRepo = productRepo;
        _basketRepo = basketRepo;
    }

    public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
    {
        // get basket from repo
        var basket = await _basketRepo.GetBasketAsync(basketId);
        if (basket == null) throw new ArgumentException("Basket does not exist");
        // get items from product repo
        var items = new List<OrderItem>();
        foreach (var item in basket.Items)
        {
            var productItem = await _productRepo.GetByIdAsync(item.Id);
            if (productItem == null) throw new ArgumentException("ProductItem does not exist");

            var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name!, productItem.PictureUrl!);
            var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
            items.Add(orderItem);
        }

        // get delivery method from repo
        var deliveryMethod = await _deliveryRepo.GetByIdAsync(deliveryMethodId);
        if (deliveryMethod == null) throw new ArgumentException("DeliveryMethod does not exist");

        // calulate subtotal
        var subtotal = items.Sum(item => item.Price * item.Quantity);

        // create order
        var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal);

        // TODO: save order to the db

        // return the order
        return order;
    }

    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Order> GetOrderById(int id, string buyerEmail)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        throw new NotImplementedException();
    }
}
