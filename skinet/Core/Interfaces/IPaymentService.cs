using Core.Entities;

namespace Core.Interfaces
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrdUpdatePaymentIntent(string basketId);
    }
}