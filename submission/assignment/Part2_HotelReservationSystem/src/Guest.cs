namespace Part2_HotelReservationSystem
{
    internal sealed class Guest
    {
        public Guest(int id, string name, string number)
        {
            if (id <= 0 || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(number))
            {
                throw new ArgumentException();
            }
            GuestId = id;
            FullName = name;
            PhoneNumber = number;
        }

        public int GuestId { get; }
        public string FullName { get; }
        public string PhoneNumber { get; }
        private readonly List<Reservation> _guestReservations = new();
        public IReadOnlyList<Reservation> GuestReservations => _guestReservations;

        public Reservation MakeReservationInTheHotel(int id, DateTimeOffset CheckinD, DateTimeOffset CheckOutD, Room room)
        {
            Reservation reservation = Reservation.CreateReservationDetails(id, CheckinD, CheckOutD, room);

            _guestReservations.Add(reservation);

            return reservation;
        }
    }
}