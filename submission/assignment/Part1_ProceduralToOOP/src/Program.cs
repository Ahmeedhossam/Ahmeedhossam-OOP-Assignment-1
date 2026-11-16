using System;

namespace Part1_ProceduralToOOP
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Store store = new Store();
            SeedSampleData(store);

            while (true)
            {
                PrintMenu();
                Console.Write("Choice: ");
                string? choice = Console.ReadLine();

                if (choice == "0")
                {
                    Console.WriteLine("Exiting...");
                    break;
                }

                // Centralized exception boundary: catches domain rule violations without crashing the app
                try
                {
                    HandleMenuChoice(choice, store);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                }
            }
        }

        private static void SeedSampleData(Store store)
        {
            store.AddCustomer(1, "Mona", "mona@mail.com", "Cairo", isVip: true);
            store.AddCustomer(2, "Omar", "omar@mail.com", "Giza", isVip: false);
            store.AddCustomer(3, "Sara", "sara@mail.com", "Alex", isVip: true);

            store.AddProduct(10, "Keyboard", 45.0m, 12);
            store.AddProduct(11, "Mouse", 20.0m, 30);
            store.AddProduct(12, "Monitor", 220.0m, 5);
            store.AddProduct(13, "USB-C Hub", 35.0m, 15);
        }

        private static void PrintMenu()
        {
            Console.WriteLine("\n===============================");
            Console.WriteLine("1) List customers");
            Console.WriteLine("2) List products");
            Console.WriteLine("3) Add customer");
            Console.WriteLine("4) Add product");
            Console.WriteLine("5) Create order");
            Console.WriteLine("6) Add line to order");
            Console.WriteLine("7) Pay order");
            Console.WriteLine("8) List orders");
            Console.WriteLine("9) Show total paid sales");
            Console.WriteLine("0) Exit");
            Console.WriteLine("===============================");
        }

        private static void HandleMenuChoice(string? choice, Store store)
        {
            switch (choice)
            {
                case "1":
                    Console.WriteLine("\n--- Customers ---");
                    foreach (var c in store.Customers)
                        Console.WriteLine(c);
                    break;

                case "2":
                    Console.WriteLine("\n--- Products ---");
                    foreach (var p in store.Products)
                        Console.WriteLine(p);
                    break;

                case "3":
                    int cId = ReadInt("Customer ID: ");
                    string name = ReadString("Name: ");
                    string email = ReadString("Email: ");
                    string city = ReadString("City: ");
                    bool isVip = ReadBool("Is VIP (true/false): ");

                    store.AddCustomer(cId, name, email, city, isVip);
                    Console.WriteLine("Customer added successfully.");
                    break;

                case "4":
                    int pId = ReadInt("Product ID: ");
                    string pName = ReadString("Product Name: ");
                    decimal price = ReadDecimal("Price: ");
                    int stock = ReadInt("Initial Stock: ");

                    store.AddProduct(pId, pName, price, stock);
                    Console.WriteLine("Product added successfully.");
                    break;

                case "5":
                    int oId = ReadInt("Order ID: ");
                    int custId = ReadInt("Customer ID: ");
                    DateOnly date = ReadDate("Order Date (yyyy-MM-dd): ");

                    store.CreateOrder(oId, custId, date);
                    Console.WriteLine("Order created successfully.");
                    break;

                case "6":
                    int targetOrderId = ReadInt("Order ID: ");
                    int prodId = ReadInt("Product ID: ");
                    int qty = ReadInt("Quantity: ");

                    store.AddLineToOrder(targetOrderId, prodId, qty);
                    Console.WriteLine("Order line added successfully.");
                    break;

                case "7":
                    int payOrderId = ReadInt("Order ID to pay: ");
                    store.PayOrder(payOrderId);
                    Console.WriteLine("Order marked as paid.");
                    break;

                case "8":
                    Console.WriteLine("\n--- Orders ---");
                    if (store.Orders.Count == 0)
                        Console.WriteLine("(no orders yet)");
                    else
                        foreach (var o in store.Orders)
                            Console.WriteLine(o);
                    break;

                case "9":
                    Console.WriteLine($"Total Paid Sales: {store.GetTotalPaidSales():F2}");
                    break;

                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }

        // Input parsing helpers so invalid console text throws a clean FormatException caught by Main
        private static int ReadInt(string prompt)
        {
            Console.Write(prompt);
            if (!int.TryParse(Console.ReadLine(), out int result))
                throw new FormatException("Invalid integer number.");
            return result;
        }

        private static decimal ReadDecimal(string prompt)
        {
            Console.Write(prompt);
            if (!decimal.TryParse(Console.ReadLine(), out decimal result))
                throw new FormatException("Invalid decimal price.");
            return result;
        }

        private static bool ReadBool(string prompt)
        {
            Console.Write(prompt);
            if (!bool.TryParse(Console.ReadLine(), out bool result))
                throw new FormatException("Please enter 'true' or 'false'.");
            return result;
        }

        private static DateOnly ReadDate(string prompt)
        {
            Console.Write(prompt);
            if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly result))
                throw new FormatException("Invalid date format. Use yyyy-MM-dd (e.g., 2026-11-16).");
            return result;
        }

        private static string ReadString(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? string.Empty;
        }
    }
}