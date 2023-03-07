namespace API.Dtos
{
    public class OrderDto
    {
        public string? BasketId { get; set; }
        public int DeliveryMethodID { get; set; }
        public AddressDto? ShipToAddress { get; set; }
    }
}