using System;

namespace Part3_BuilderPattern
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Invoice invoice = new Invoice(
                "INV-2026-001",
                "Ahmed Hassan",
                "ahmed.hassan@example.com",
                "01012345678",
                "15 El-Tahrir St",
                "Cairo",
                "Cairo Governorate",
                "11511",
                "Egypt",
                "24 Nasr Road",
                "Cairo",
                "Cairo Governorate",
                "11765",
                "Egypt",
                DateTimeOffset.UtcNow,
                "Credit Card",
                "EGP",
                2000m,
                200m,
                252m,
                2052m
            );

            Console.WriteLine($"Invoice: {invoice.InvoiceId} | Customer: {invoice.CustomerName}");
            Console.WriteLine($"Billing City: {invoice.BillingCity} -> Shipping City: {invoice.ShippingCity}");
            Console.WriteLine($"Total: {invoice.TotalAmount:F2} {invoice.Currency}");
        }
    }
}