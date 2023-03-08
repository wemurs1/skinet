using System.Linq.Expressions;
using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrdersWithItemsndOrderingSpecification : BaseSpecification<Order>
    {
        public OrdersWithItemsndOrderingSpecification(string email) : base(o => o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems!);
            AddInclude(o => o.DeliveryMethod!);
            AddOrderByDescending(o => o.OrderDate);
        }

        public OrdersWithItemsndOrderingSpecification(int orderId, string email)
            : base(o => o.Id == orderId && o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems!);
            AddInclude(o => o.DeliveryMethod!);
        }
    }
}