internal sealed class Product
{
    public Product(int id, string name, decimal price, int productStock)

    {
        if (id <= 0 || price <= 0 || productStock < 0)
        {
            throw new ArgumentOutOfRangeException();
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentNullException();
        }
        Id = id;
        Name = name;
        Price = price;
        Stock = productStock;
    }

    public int Id { get; init; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }

    public override string ToString()
    {
        return $"# {this.Id} {this.Name} price = {this.Price} Stock = {this.Stock} \n ";
    }

    public bool CanWeTakeQuantityFromStock(int quantity)
    {
        if (quantity > 0 && this.Stock >= quantity)
        {
            this.Stock -= quantity;
            return true;
        }
        return false;
    }

    public void RestoreStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity to restore must be positive.");
        }
        Stock += quantity;
    }
}