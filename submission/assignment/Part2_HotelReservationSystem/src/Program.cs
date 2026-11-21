using System;

namespace Part2_HotelReservationSystem
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Hotel hotel = new Hotel();
            SeedSampleData(hotel);

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

                try
                {
                    HandleMenuChoice(choice, hotel);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                }
            }
        }

        private static void SeedSampleData(Hotel hotel)
        {
            hotel.RegisterGuest(1, "Ahmed Hassan", "01012345678");
            hotel.RegisterGuest(2, "Nour El-Din", "01198765432");
            hotel.RegisterGuest(3, "Mariam Adel", "01234567890");

            hotel.AddRoom(101, 500m, isUnderMaintenance: false, Room.RoomType.Single);
            hotel.AddRoom(102, 850m, isUnderMaintenance: false, Room.RoomType.Double);
            hotel.AddRoom(201, 1500m, isUnderMaintenance: false, Room.RoomType.Suite);
            hotel.AddRoom(202, 900m, isUnderMaintenance: true, Room.RoomType.Double);

            DateTimeOffset baseDate = new DateTimeOffset(2026, 11, 20, 14, 0, 0, TimeSpan.Zero);
            hotel.BookRoom(1001, guestId: 1, roomNumber: 101, checkIn: baseDate, checkOut: baseDate.AddDays(3));
            hotel.ConfirmReservation(1001);
        }

        private static void PrintMenu()
        {
            Console.WriteLine("\n==========================================");
            Console.WriteLine("1) List guests & their reservation history");
            Console.WriteLine("2) List rooms");
            Console.WriteLine("3) Register new guest");
            Console.WriteLine("4) Add new room");
            Console.WriteLine("5) Book a room (Create reservation)");
            Console.WriteLine("6) Confirm reservation");
            Console.WriteLine("7) Check-in reservation");
            Console.WriteLine("8) Check-out reservation");
            Console.WriteLine("9) Cancel reservation");
            Console.WriteLine("10) Start / End room maintenance");
            Console.WriteLine("11) Update room nightly rate");
            Console.WriteLine("12) List all hotel reservations");
            Console.WriteLine("0) Exit");
            Console.WriteLine("==========================================");
        }

        private static void HandleMenuChoice(string? choice, Hotel hotel)
        {
            switch (choice)
            {
                case "1":
                    Console.WriteLine("\n--- Guests & History ---");
                    foreach (var g in hotel.Guests)
                    {
                        Console.WriteLine($"ID: {g.GuestId} | Name: {g.FullName} | Phone: {g.PhoneNumber} | Reservations Count: {g.GuestReservations.Count}");
                        foreach (var r in g.GuestReservations)
                        {
                            Console.WriteLine($"   -> Res #{r.ReservationId}: Room {r.Room.RoomNumber} | {r.CheckInDate:yyyy-MM-dd} to {r.CheckOutDate:yyyy-MM-dd} | Status: {r.Status} | Total: {r.ReservationTotalCost:F2}");
                        }
                    }
                    break;

                case "2":
                    Console.WriteLine("\n--- Rooms ---");
                    foreach (var r in hotel.Rooms)
                    {
                        Console.WriteLine($"Room #{r.RoomNumber} | Type: {r.Room_Type} | Rate: {r.NightlyRate:F2} | Maintenance: {r.RoomIsUnderMaintaince}");
                    }
                    break;

                case "3":
                    int gId = ReadInt("Guest ID: ");
                    string name = ReadString("Full Name: ");
                    string phone = ReadString("Phone Number: ");
                    hotel.RegisterGuest(gId, name, phone);
                    Console.WriteLine("Guest registered successfully.");
                    break;

                case "4":
                    int rNum = ReadInt("Room Number: ");
                    decimal rate = ReadDecimal("Nightly Rate: ");
                    bool maint = ReadBool("Under Maintenance (true/false): ");
                    Room.RoomType rType = ReadRoomType("Room Type (Single / Double / Suite): ");
                    hotel.AddRoom(rNum, rate, maint, rType);
                    Console.WriteLine("Room added successfully.");
                    break;

                case "5":
                    int resId = ReadInt("Reservation ID: ");
                    int guestId = ReadInt("Guest ID: ");
                    int roomNum = ReadInt("Room Number: ");
                    DateTimeOffset checkIn = ReadDate("Check-In Date (yyyy-MM-dd): ");
                    DateTimeOffset checkOut = ReadDate("Check-Out Date (yyyy-MM-dd): ");
                    Reservation created = hotel.BookRoom(resId, guestId, roomNum, checkIn, checkOut);
                    Console.WriteLine($"Reservation #{created.ReservationId} created (Pending). Total Cost: {created.ReservationTotalCost:F2}");
                    break;

                case "6":
                    hotel.ConfirmReservation(ReadInt("Reservation ID to confirm: "));
                    Console.WriteLine("Reservation confirmed.");
                    break;

                case "7":
                    hotel.CheckInReservation(ReadInt("Reservation ID to check-in: "));
                    Console.WriteLine("Guest checked in.");
                    break;

                case "8":
                    hotel.CheckOutReservation(ReadInt("Reservation ID to check-out: "));
                    Console.WriteLine("Guest checked out.");
                    break;

                case "9":
                    hotel.CancelReservation(ReadInt("Reservation ID to cancel: "));
                    Console.WriteLine("Reservation cancelled.");
                    break;

                case "10":
                    int targetRoom = ReadInt("Room Number: ");
                    bool start = ReadBool("Start maintenance? (true = start, false = end): ");
                    if (start) hotel.StartRoomMaintenance(targetRoom);
                    else hotel.EndRoomMaintenance(targetRoom);
                    Console.WriteLine("Room maintenance status updated.");
                    break;

                case "11":
                    int rateRoom = ReadInt("Room Number: ");
                    decimal newRate = ReadDecimal("New Nightly Rate: ");
                    hotel.UpdateRoomRate(rateRoom, newRate);
                    Console.WriteLine("Nightly rate updated.");
                    break;

                case "12":
                    Console.WriteLine("\n--- All Hotel Reservations ---");
                    foreach (var r in hotel.Reservations)
                    {
                        Console.WriteLine($"Res #{r.ReservationId} | Room {r.Room.RoomNumber} | {r.CheckInDate:yyyy-MM-dd} -> {r.CheckOutDate:yyyy-MM-dd} | Status: {r.Status} | Total: {r.ReservationTotalCost:F2}");
                    }
                    break;

                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }

        private static int ReadInt(string prompt)
        {
            Console.Write(prompt);
            if (!int.TryParse(Console.ReadLine(), out int result))
                throw new FormatException("Invalid integer value.");
            return result;
        }

        private static decimal ReadDecimal(string prompt)
        {
            Console.Write(prompt);
            if (!decimal.TryParse(Console.ReadLine(), out decimal result))
                throw new FormatException("Invalid decimal value.");
            return result;
        }

        private static bool ReadBool(string prompt)
        {
            Console.Write(prompt);
            if (!bool.TryParse(Console.ReadLine(), out bool result))
                throw new FormatException("Please enter 'true' or 'false'.");
            return result;
        }

        private static DateTimeOffset ReadDate(string prompt)
        {
            Console.Write(prompt);
            if (!DateTimeOffset.TryParse(Console.ReadLine(), out DateTimeOffset result))
                throw new FormatException("Invalid date format. Use yyyy-MM-dd (e.g., 2026-11-25).");
            return result;
        }

        private static Room.RoomType ReadRoomType(string prompt)
        {
            Console.Write(prompt);
            if (!Enum.TryParse<Room.RoomType>(Console.ReadLine(), ignoreCase: true, out Room.RoomType result))
                throw new FormatException("Invalid room type. Enter Single, Double, or Suite.");
            return result;
        }

        private static string ReadString(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? string.Empty;
        }
    }
}