internal sealed class OrderLine
{
    private OrderLine(int buyQuantity, Product productt)
    {
        BuyQuantity = buyQuantity;
        this.Product = productt;
        this.OrderLineTotalPrice = this.BuyQuantity * Product.Price;
    }

    public int BuyQuantity { get; }
    public Product Product { get; }
    public decimal OrderLineTotalPrice;

    public static OrderLine CreateOrderLine(int quantity, Product product)
    {
        if (quantity > 0 && product.CanWeTakeQuantityFromStock(quantity))
        {
            return new OrderLine(quantity, product);
        }
        return null;
    }

    public override string? ToString()
    {
        return $"-{this.Product.Name} *{this.BuyQuantity} {Product.Price} = {this.OrderLineTotalPrice}  ";
    }
}