namespace Core.Entities.OrderAggregate;

public class ProductItemOrdered
{
    public ProductItemOrdered()
    {
    }

    public ProductItemOrdered(int productitemId, string productName, string pictureUrl)
    {
        ProductitemId = productitemId;
        ProductName = productName;
        PictureUrl = pictureUrl;
    }

    public int ProductitemId { get; set; }
    public string? ProductName { get; set; }
    public string? PictureUrl { get; set; }
}
