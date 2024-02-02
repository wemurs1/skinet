using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IBasketRepository _basketRepo;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IBasketRepository basketRepo, IUnitOfWork unitOfWork)
    {
        _basketRepo = basketRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<Order?> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
    {
        // get basket from repo
        var basket = await _basketRepo.GetBasketAsync(basketId);
        if (basket == null) throw new ArgumentException("Basket does not exist");
        // get items from product repo
        var items = new List<OrderItem>();
        foreach (var item in basket.Items)
        {
            var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
            if (productItem == null) throw new ArgumentException("ProductItem does not exist");

            var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name!, productItem.PictureUrl!);
            var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
            items.Add(orderItem);
        }

        // get delivery method from repo
        var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
        if (deliveryMethod == null) throw new ArgumentException("DeliveryMethod does not exist");

        // calulate subtotal
        var subtotal = items.Sum(item => item.Price * item.Quantity);

        // create order
        var order = new Order(items, buyerEmail, shippingAddress, deliveryMethod, subtotal);

        // save order to the db
        _unitOfWork.Repository<Order>().Add(order);
        var result = await _unitOfWork.Complete();
        if (result <= 0) return null;

        // delete basket
        await _basketRepo.DeleteBasketAsync(basketId);

        // return the order
        return order;
    }

    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
        return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
    }

    public async Task<Order?> GetOrderById(int id, string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);
        return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
    }

    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);
        return await _unitOfWork.Repository<Order>().ListAsync(spec);
    }
}
