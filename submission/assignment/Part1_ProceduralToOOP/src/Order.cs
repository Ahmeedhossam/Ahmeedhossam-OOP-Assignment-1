using System.Text;

internal class Order
{
    private Order(int orderId, Customer customer, List<OrderLine> orderLine, bool isPaid, DateOnly date)
    {
        if (orderId <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(orderId), "Order ID must be greater than zero.");
        }
        if (customer is null)
        {
            throw new ArgumentNullException(nameof(customer), "Customer cannot be null.");
        }
        if (orderLine is null)
        {
            throw new ArgumentNullException(nameof(orderLine), "Order lines list cannot be null.");
        }
        if (isPaid && orderLine.Count == 0)
        {
            throw new InvalidOperationException("Cannot create an already paid order with no items.");
        }
        OrderId = orderId;
        Customer = customer;
        _orderLines = orderLine;
        OrderDate = date;
        Ispaid = isPaid;
    }

    public int OrderId { get; }
    public Customer Customer { get; }
    private readonly List<OrderLine> _orderLines;
    public IReadOnlyList<OrderLine> OrderLines => _orderLines;

    public decimal TotalPrice

    {
        get
        {
            decimal total = 0m;
            foreach (var line in _orderLines)
            {
                total += line.OrderLineTotalPrice;
            }
            return this.Customer.IsVip ? total * 0.90m : total;
        }
    }

    public DateOnly OrderDate;
    public bool Ispaid { get; private set; }

    public static Order CreateOrder(int id, Customer customer, List<OrderLine> list, bool paid, DateOnly date)
    {
        return new Order(id, customer, list, paid, date);
    }

    public void AddOrderLineToOrder(OrderLine orderLine)
    {
        if (this.Ispaid is true)
        {
            throw new InvalidOperationException();
        }
        if (orderLine is null)
        {
            throw new ArgumentNullException();
        }
        this._orderLines.Add(orderLine);
    }

    public void RemoveOrderLineToOrder(OrderLine orderLine)
    {
        if (this.Ispaid is true)
        {
            throw new InvalidOperationException();
        }
        if (orderLine is null)
        {
            throw new ArgumentNullException(nameof(orderLine));
        }

        if (this._orderLines.Remove(orderLine))
        {
            orderLine.Product.RestoreStock(orderLine.BuyQuantity);
        }
    }

    public void MakeOrderPaid()
    {
        if (this.Ispaid)
        {
            throw new InvalidOperationException("Order is already paid.");
        }
        if (this._orderLines.Count == 0)
        {
            throw new InvalidOperationException("Cannot pay an empty order.");
        }
        this.Ispaid = true;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"=== Order #{OrderId} ===");
        sb.AppendLine($"Date: {OrderDate}");
        sb.AppendLine($"Customer: {Customer.Name} (#{Customer.Id})");
        sb.AppendLine($"Paid: {(Ispaid ? "yes" : "no")}");
        sb.AppendLine("Lines:");

        if (_orderLines.Count == 0)
        {
            sb.AppendLine("  (no items)");
        }
        else
        {
            foreach (OrderLine line in _orderLines)
            {
                sb.AppendLine(line.ToString());
            }
        }

        sb.AppendLine($"Total: {TotalPrice:F2}");

        return sb.ToString();
    }
}