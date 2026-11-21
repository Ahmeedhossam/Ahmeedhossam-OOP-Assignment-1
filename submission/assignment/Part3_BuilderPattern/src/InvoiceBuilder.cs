namespace Part3_BuilderPattern
{
    internal sealed class InvoiceBuilder
    {
        private readonly string _invoiceId;
        private readonly string _customerName;
        private readonly string _customerEmail;

        private string _customerPhone = "Unknown";

        private string _billingStreet = "Unknown";
        private string _billingCity = "Unknown";
        private string _billingState = "Unknown";
        private string _billingZipCode = "Unknown";
        private string _billingCountry = "Unknown";

        private string _shippingStreet = "Unknown";
        private string _shippingCity = "Unknown";
        private string _shippingState = "Unknown";
        private string _shippingZipCode = "Unknown";
        private string _shippingCountry = "Unknown";

        private DateTimeOffset _orderDate = DateTimeOffset.UtcNow;
        private string _paymentMethod = "Cash";
        private string _currency = "EGP";
        private decimal _subTotal = 0m;
        private decimal _discountAmount = 0m;
        private decimal _taxAmount = 0m;
        private decimal? _totalAmount = null;

        public InvoiceBuilder(string invoiceId, string customerName, string customerEmail)
        {
            if (string.IsNullOrWhiteSpace(invoiceId))
                throw new ArgumentException("Invoice ID cannot be empty.", nameof(invoiceId));

            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentException("Customer name cannot be empty.", nameof(customerName));

            if (string.IsNullOrWhiteSpace(customerEmail) || !customerEmail.Contains('@'))
                throw new ArgumentException("A valid customer email is required.", nameof(customerEmail));

            _invoiceId = invoiceId;
            _customerName = customerName;
            _customerEmail = customerEmail;
        }

        public InvoiceBuilder WithCustomerPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Phone number cannot be empty.", nameof(phone));

            _customerPhone = phone;
            return this;
        }

        public InvoiceBuilder WithBillingStreet(string street)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Billing street cannot be empty.", nameof(street));

            _billingStreet = street;
            return this;
        }

        public InvoiceBuilder WithBillingCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("Billing city cannot be empty.", nameof(city));

            _billingCity = city;
            return this;
        }

        public InvoiceBuilder WithBillingState(string state)
        {
            if (string.IsNullOrWhiteSpace(state))
                throw new ArgumentException("Billing state cannot be empty.", nameof(state));

            _billingState = state;
            return this;
        }

        public InvoiceBuilder WithBillingZipCode(string zipCode)
        {
            if (string.IsNullOrWhiteSpace(zipCode))
                throw new ArgumentException("Billing zip code cannot be empty.", nameof(zipCode));

            _billingZipCode = zipCode;
            return this;
        }

        public InvoiceBuilder WithBillingCountry(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
                throw new ArgumentException("Billing country cannot be empty.", nameof(country));

            _billingCountry = country;
            return this;
        }

        public InvoiceBuilder WithShippingStreet(string street)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Shipping street cannot be empty.", nameof(street));

            _shippingStreet = street;
            return this;
        }

        public InvoiceBuilder WithShippingCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("Shipping city cannot be empty.", nameof(city));

            _shippingCity = city;
            return this;
        }

        public InvoiceBuilder WithShippingState(string state)
        {
            if (string.IsNullOrWhiteSpace(state))
                throw new ArgumentException("Shipping state cannot be empty.", nameof(state));

            _shippingState = state;
            return this;
        }

        public InvoiceBuilder WithShippingZipCode(string zipCode)
        {
            if (string.IsNullOrWhiteSpace(zipCode))
                throw new ArgumentException("Shipping zip code cannot be empty.", nameof(zipCode));

            _shippingZipCode = zipCode;
            return this;
        }

        public InvoiceBuilder WithShippingCountry(string country)
        {
            if (string.IsNullOrWhiteSpace(country))
                throw new ArgumentException("Shipping country cannot be empty.", nameof(country));

            _shippingCountry = country;
            return this;
        }

        public InvoiceBuilder WithOrderDate(DateTimeOffset orderDate)
        {
            _orderDate = orderDate;
            return this;
        }

        public InvoiceBuilder WithPaymentMethod(string paymentMethod)
        {
            if (string.IsNullOrWhiteSpace(paymentMethod))
                throw new ArgumentException("Payment method cannot be empty.", nameof(paymentMethod));

            _paymentMethod = paymentMethod;
            return this;
        }

        public InvoiceBuilder WithCurrency(string currency)
        {
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency cannot be empty.", nameof(currency));

            _currency = currency;
            return this;
        }

        public InvoiceBuilder WithSubTotal(decimal subTotal)
        {
            if (subTotal < 0m)
                throw new ArgumentOutOfRangeException(nameof(subTotal), "SubTotal cannot be negative.");

            _subTotal = subTotal;
            return this;
        }

        public InvoiceBuilder WithDiscountAmount(decimal discountAmount)
        {
            if (discountAmount < 0m)
                throw new ArgumentOutOfRangeException(nameof(discountAmount), "Discount amount cannot be negative.");

            _discountAmount = discountAmount;
            return this;
        }

        public InvoiceBuilder WithTaxAmount(decimal taxAmount)
        {
            if (taxAmount < 0m)
                throw new ArgumentOutOfRangeException(nameof(taxAmount), "Tax amount cannot be negative.");

            _taxAmount = taxAmount;
            return this;
        }

        public InvoiceBuilder WithTotalAmount(decimal totalAmount)
        {
            if (totalAmount < 0m)
                throw new ArgumentOutOfRangeException(nameof(totalAmount), "Total amount cannot be negative.");

            _totalAmount = totalAmount;
            return this;
        }

        public Invoice Build()
        {
            if (_discountAmount > _subTotal)
                throw new InvalidOperationException("Discount amount cannot exceed the subtotal.");

            decimal finalTotal = _totalAmount ?? (_subTotal - _discountAmount + _taxAmount);

            if (finalTotal < 0m)
                throw new InvalidOperationException("Final total amount cannot be negative.");

            return new Invoice(
                _invoiceId,
                _customerName,
                _customerEmail,
                _customerPhone,
                _billingStreet,
                _billingCity,
                _billingState,
                _billingZipCode,
                _billingCountry,
                _shippingStreet,
                _shippingCity,
                _shippingState,
                _shippingZipCode,
                _shippingCountry,
                _orderDate,
                _paymentMethod,
                _currency,
                _subTotal,
                _discountAmount,
                _taxAmount,
                finalTotal);
        }
    }
}