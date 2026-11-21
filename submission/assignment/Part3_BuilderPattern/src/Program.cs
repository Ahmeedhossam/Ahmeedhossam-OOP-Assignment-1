using System;

namespace Part3_BuilderPattern
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Invoice validInvoice = new InvoiceBuilder("INV-2026-001", "Ahmed Hassan", "ahmed.hassan@example.com")
                    .WithCustomerPhone("01012345678")
                    .WithBillingStreet("15 El-Tahrir St")
                    .WithBillingCity("Cairo")
                    .WithBillingCountry("Egypt")
                    .WithPaymentMethod("Credit Card")
                    .WithSubTotal(2000m)
                    .WithDiscountAmount(200m)
                    .WithTaxAmount(252m)
                    .Build();

            Console.WriteLine($"Created: {validInvoice.InvoiceId} for {validInvoice.CustomerName}");
            Console.WriteLine($"Billing: {validInvoice.BillingStreet}, {validInvoice.BillingCity} | Shipping: {validInvoice.ShippingCity}");
            Console.WriteLine($"Total: {validInvoice.TotalAmount:F2} {validInvoice.Currency}");

            try
            {
                Invoice invalidInvoice = new InvoiceBuilder("INV-2026-002", "Sara Ali", "sara@example.com")
                    .WithSubTotal(500m)
                    .WithDiscountAmount(800m)
                    .Build();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Validation Blocked Build: {ex.Message}");
            }
        }
    }
}