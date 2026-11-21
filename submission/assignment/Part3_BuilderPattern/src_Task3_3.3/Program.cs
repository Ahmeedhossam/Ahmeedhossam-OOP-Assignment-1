using System;

namespace Part3_ComposedBuilder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            MemberDetails member = new MemberDetailsBuilder("INV-2026-001", "Ahmed Hassan", "ahmed@example.com", "01012345678")
                .Build();

            Address billing = new AddressBuilder()
                .WithStreet("15 El-Tahrir St")
                .WithCity("Cairo")
                .WithState("Cairo Governorate")
                .WithZipCode("11511")
                .WithCountry("Egypt")
                .Build();

            Address shipping = new AddressBuilder()
                .WithStreet("24 Nasr Road")
                .WithCity("Cairo")
                .WithCountry("Egypt")
                .Build();

            OrderDetails order = new OrderBuilder()
                .WithPaymentMethod("Credit Card")
                .WithCurrency("EGP")
                .WithSubTotal(2000m)
                .WithDiscountAmount(200m)
                .WithTaxAmount(252m)
                .Build();

            Invoice invoice = new InvoiceBuilder()
                .WithMemberDetails(member)
                .WithBillingAddress(billing)
                .WithShippingAddress(shipping)
                .WithOrderDetails(order)
                .Build();

            Console.WriteLine($"Invoice: {invoice.MemberDetails.InvoiceId} | Customer: {invoice.MemberDetails.CustomerName}");
            Console.WriteLine($"Billing: {invoice.BillingAddress.Street}, {invoice.BillingAddress.City}");
            Console.WriteLine($"Shipping: {invoice.ShippingAddress.Street}, {invoice.ShippingAddress.City}");
            Console.WriteLine($"Total: {invoice.OrderDetails.TotalAmount:F2} {invoice.OrderDetails.Currency}");
        }
    }

    internal sealed class Invoice
    {
        internal Invoice(MemberDetails memberDetails, Address billingAddress, Address shippingAddress, OrderDetails orderDetails)
        {
            MemberDetails = memberDetails;
            BillingAddress = billingAddress;
            ShippingAddress = shippingAddress;
            OrderDetails = orderDetails;
        }

        public MemberDetails MemberDetails { get; }
        public Address BillingAddress { get; }
        public Address ShippingAddress { get; }
        public OrderDetails OrderDetails { get; }
    }

    internal sealed class MemberDetails
    {
        internal MemberDetails(string invoiceId, string customerName, string customerEmail, string customerPhone)
        {
            InvoiceId = invoiceId;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            CustomerPhone = customerPhone;
        }

        public string InvoiceId { get; }
        public string CustomerName { get; }
        public string CustomerEmail { get; }
        public string CustomerPhone { get; }
    }

    internal sealed class MemberDetailsBuilder
    {
        public MemberDetailsBuilder(string invoiceId, string customerName, string customerEmail, string customerPhone)
        {
            if (string.IsNullOrWhiteSpace(invoiceId))
                throw new ArgumentException("Invoice ID cannot be empty.", nameof(invoiceId));
            if (string.IsNullOrWhiteSpace(customerPhone))
                throw new ArgumentException("customer phone cannot be empty.", nameof(customerPhone));

            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentException("Customer name cannot be empty.", nameof(customerName));

            if (string.IsNullOrWhiteSpace(customerEmail) || !customerEmail.Contains('@'))
                throw new ArgumentException("Valid customer email is required.", nameof(customerEmail));
            _invoiceId = invoiceId;
            _customerName = customerName;
            _customerEmail = customerEmail;
            _customerPhone = customerPhone;
        }

        private string _invoiceId { get; }
        private string _customerName { get; }
        private string _customerEmail { get; }
        private string _customerPhone { get; }

        public MemberDetails Build()
        {
            if (string.IsNullOrWhiteSpace(_invoiceId) || string.IsNullOrWhiteSpace(_customerName) ||
                string.IsNullOrWhiteSpace(_customerEmail) || string.IsNullOrWhiteSpace(_customerPhone))
            {
                throw new ArgumentException(nameof(_invoiceId));
            }
            return new MemberDetails(_invoiceId, _customerName, _customerEmail, _customerPhone);
        }
    }

    internal sealed class Address
    {
        public string Street { get; }

        public string City { get; }
        public string Country { get; }
        public string Zipcode { get; }
        public string State { get; }

        internal Address(string street, string city, string state, string zipcode, string country)
        {
            Street = street;
            City = city;
            Country = country;
            Zipcode = zipcode;
            State = state;
        }
    }

    internal sealed class AddressBuilder
    {
        private string _street = "Unknown";
        private string _city = "Unknown";
        private string _state = "Unknown";
        private string _zipCode = "Unknown";
        private string _country = "Unknown";

        public AddressBuilder WithStreet(string street)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Street cannot be empty.", nameof(street));

            _street = street;
            return this;
        }

        public AddressBuilder WithCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City cannot be empty.", nameof(city));

            _city = city;
            return this;
        }

        public AddressBuilder WithState(string state)
        {
            if (string.IsNullOrWhiteSpace(state))
                throw new ArgumentException("State cannot be empty.", nameof(state));

            _state = state;
            return this;
        }

        public AddressBuilder WithZipCode(string zipCode)
        {
            if (string.IsNullOrWhiteSpace(zipCode))
                throw new ArgumentException("Zip code cannot be empty.", nameof(zipCode));

            _zipCode = zipCode;
            return this;
        }

        public AddressBuilder WithCountry(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
                throw new ArgumentException("Country cannot be empty.", nameof(country));

            _country = country;
            return this;
        }

        public Address Build()
        {
            if (_street == "Unknown" || _city == "Unknown")
            {
                throw new InvalidOperationException("At least a street and city must be provided to build an Address.");
            }

            return new Address(_street, _city, _state, _zipCode, _country);
        }
    }

    internal sealed class OrderDetails
    {
        public DateTimeOffset OrderDate { get; }
        public string PaymentMethod { get; }
        public string Currency { get; }
        public decimal SubTotal { get; }
        public decimal DiscountAmount { get; }
        public decimal TaxAmount { get; }
        public decimal TotalAmount { get; }

        internal OrderDetails(
            DateTimeOffset orderDate,
            string paymentMethod,
            string currency,
            decimal subTotal,
            decimal discountAmount,
            decimal taxAmount,
            decimal totalAmount)
        {
            OrderDate = orderDate;
            PaymentMethod = paymentMethod;
            Currency = currency;
            SubTotal = subTotal;
            DiscountAmount = discountAmount;
            TaxAmount = taxAmount;
            TotalAmount = totalAmount;
        }
    }

    internal sealed class OrderBuilder
    {
        private DateTimeOffset _orderDate = DateTimeOffset.UtcNow;
        private string _paymentMethod = "Unknown";
        private string _currency = "Unknown";
        private decimal _subTotal = 0m;
        private decimal _discountAmount = 0m;
        private decimal _taxAmount = 0m;
        private decimal? _totalAmount = null;

        public OrderBuilder WithOrderDate(DateTimeOffset orderDate)
        {
            if (orderDate == default)
                throw new ArgumentException("Order date must be a valid date.", nameof(orderDate));

            _orderDate = orderDate;
            return this;
        }

        public OrderBuilder WithPaymentMethod(string paymentMethod)
        {
            if (string.IsNullOrWhiteSpace(paymentMethod))
                throw new ArgumentException("Payment method cannot be empty.", nameof(paymentMethod));

            _paymentMethod = paymentMethod;
            return this;
        }

        public OrderBuilder WithCurrency(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency cannot be empty.", nameof(currency));

            _currency = currency;
            return this;
        }

        public OrderBuilder WithSubTotal(decimal subTotal)
        {
            if (subTotal < 0m)
                throw new ArgumentOutOfRangeException(nameof(subTotal), "SubTotal cannot be negative.");

            _subTotal = subTotal;
            return this;
        }

        public OrderBuilder WithDiscountAmount(decimal discountAmount)
        {
            if (discountAmount < 0m)
                throw new ArgumentOutOfRangeException(nameof(discountAmount), "Discount amount cannot be negative.");

            _discountAmount = discountAmount;
            return this;
        }

        public OrderBuilder WithTaxAmount(decimal taxAmount)
        {
            if (taxAmount < 0m)
                throw new ArgumentOutOfRangeException(nameof(taxAmount), "Tax amount cannot be negative.");

            _taxAmount = taxAmount;
            return this;
        }

        public OrderBuilder WithTotalAmount(decimal totalAmount)
        {
            if (totalAmount < 0m)
                throw new ArgumentOutOfRangeException(nameof(totalAmount), "Total amount cannot be negative.");

            _totalAmount = totalAmount;
            return this;
        }

        public OrderDetails Build()
        {
            if (_paymentMethod == "Unknown" || _currency == "Unknown")
                throw new InvalidOperationException("Both payment method and currency must be specified.");

            if (_discountAmount > _subTotal)
                throw new InvalidOperationException("Discount amount cannot exceed the subtotal.");

            decimal finalTotal = _totalAmount ?? (_subTotal - _discountAmount + _taxAmount);

            if (finalTotal < 0m)
                throw new InvalidOperationException("Final total amount cannot be negative.");

            return new OrderDetails(
                _orderDate,
                _paymentMethod,
                _currency,
                _subTotal,
                _discountAmount,
                _taxAmount,
                finalTotal);
        }
    }

    internal sealed class InvoiceBuilder
    {
        private MemberDetails? _memberDetails;
        private Address? _billingAddress;
        private Address? _shippingAddress;
        private OrderDetails? _orderDetails;

        public InvoiceBuilder WithMemberDetails(MemberDetails memberDetails)
        {
            _memberDetails = memberDetails ?? throw new ArgumentNullException(nameof(memberDetails));
            return this;
        }

        public InvoiceBuilder WithBillingAddress(Address billingAddress)
        {
            _billingAddress = billingAddress ?? throw new ArgumentNullException(nameof(billingAddress));
            return this;
        }

        public InvoiceBuilder WithShippingAddress(Address shippingAddress)
        {
            _shippingAddress = shippingAddress ?? throw new ArgumentNullException(nameof(shippingAddress));
            return this;
        }

        public InvoiceBuilder WithOrderDetails(OrderDetails orderDetails)
        {
            _orderDetails = orderDetails ?? throw new ArgumentNullException(nameof(orderDetails));
            return this;
        }

        public Invoice Build()
        {
            if (_memberDetails is null)
                throw new InvalidOperationException("Member details are required to build an Invoice.");

            if (_billingAddress is null)
                throw new InvalidOperationException("Billing address is required to build an Invoice.");

            if (_orderDetails is null)
                throw new InvalidOperationException("Order details are required to build an Invoice.");

            Address finalShippingAddress = _shippingAddress ?? _billingAddress;

            return new Invoice(_memberDetails, _billingAddress, finalShippingAddress, _orderDetails);
        }
    }
}