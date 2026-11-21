internal sealed class Store
{
    private readonly List<Customer> _customers = new();

    private readonly List<Product> _products = new();
    private readonly List<Order> _orders = new();

    public IReadOnlyList<Customer> Customers => _customers;
    public IReadOnlyList<Product> Products => _products;
    public IReadOnlyList<Order> Orders => _orders;

    public Customer AddCustomer(int id, string name, string email, string city, bool isVip)
    {
        if (FindCustomerById(id) is not null)
        {
            throw new InvalidOperationException($"Customer ID {id} already exists.");
        }

        var customer = new Customer(id, name, email, city, isVip);
        _customers.Add(customer);
        return customer;
    }

    public Product AddProduct(int id, string name, decimal price, int stock)
    {
        if (FindProductById(id) is not null)
        {
            throw new InvalidOperationException($"Product ID {id} already exists.");
        }

        var product = new Product(id, name, price, stock);
        _products.Add(product);
        return product;
    }

    public Order CreateOrder(int orderId, int customerId, DateOnly date)
    {
        if (FindOrderById(orderId) is not null)
        {
            throw new InvalidOperationException($"Order ID {orderId} already exists.");
        }

        Customer? customer = FindCustomerById(customerId);
        if (customer is null)
        {
            throw new InvalidOperationException($"Customer ID {customerId} not found.");
        }

        var order = Order.CreateOrder(orderId, customer, new List<OrderLine>(), paid: false, date);
        _orders.Add(order);
        return order;
    }

    public void AddLineToOrder(int orderId, int productId, int quantity)
    {
        Order? order = FindOrderById(orderId);
        if (order is null)
        {
            throw new InvalidOperationException($"Order ID {orderId} not found.");
        }

        Product? product = FindProductById(productId);
        if (product is null)
        {
            throw new InvalidOperationException($"Product ID {productId} not found.");
        }

        OrderLine? line = OrderLine.CreateOrderLine(quantity, product);
        if (line is null)
        {
            throw new InvalidOperationException($"Invalid quantity or insufficient stock for product '{product.Name}'.");
        }

        order.AddOrderLineToOrder(line);
    }

    public void PayOrder(int orderId)
    {
        Order? order = FindOrderById(orderId);
        if (order is null)
        {
            throw new InvalidOperationException($"Order ID {orderId} not found.");
        }

        order.MakeOrderPaid();
    }

    public decimal GetTotalPaidSales()
    {
        decimal totalSales = 0m;
        foreach (Order order in _orders)
        {
            if (order.Ispaid)
            {
                totalSales += order.TotalPrice;
            }
        }
        return totalSales;
    }

    private Customer? FindCustomerById(int id)
    {
        foreach (Customer c in _customers)
        {
            if (c.Id == id) return c;
        }
        return null;
    }

    private Product? FindProductById(int id)
    {
        foreach (Product p in _products)
        {
            if (p.Id == id) return p;
        }
        return null;
    }

    private Order? FindOrderById(int id)
    {
        foreach (Order o in _orders)
        {
            if (o.OrderId == id) return o;
        }
        return null;
    }
}