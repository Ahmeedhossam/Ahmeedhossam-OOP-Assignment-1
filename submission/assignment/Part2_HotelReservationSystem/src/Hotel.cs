namespace Part2_HotelReservationSystem
{
    internal sealed class Hotel
    {
        private readonly List<Guest> _guests = new();
        private readonly List<Room> _rooms = new();
        private readonly List<Reservation> _reservations = new();

        public IReadOnlyList<Guest> Guests => _guests;
        public IReadOnlyList<Room> Rooms => _rooms;
        public IReadOnlyList<Reservation> Reservations => _reservations;

        public Guest RegisterGuest(int guestId, string fullName, string phoneNumber)
        {
            if (FindGuestById(guestId) is not null)
            {
                throw new InvalidOperationException($"Guest ID {guestId} already exists.");
            }

            var guest = new Guest(guestId, fullName, phoneNumber);
            _guests.Add(guest);
            return guest;
        }

        public Room AddRoom(int roomNumber, decimal nightlyRate, bool isUnderMaintenance, Room.RoomType roomType)
        {
            if (FindRoomByNumber(roomNumber) is not null)
            {
                throw new InvalidOperationException($"Room number {roomNumber} already exists.");
            }

            Room room = Room.CreateRoom(roomNumber, nightlyRate, isUnderMaintenance, roomType);
            _rooms.Add(room);
            return room;
        }

        public Reservation BookRoom(int reservationId, int guestId, int roomNumber, DateTimeOffset checkIn, DateTimeOffset checkOut)
        {
            if (FindReservationById(reservationId) is not null)
            {
                throw new InvalidOperationException($"Reservation ID {reservationId} already exists.");
            }

            Guest guest = FindGuestById(guestId)
                ?? throw new InvalidOperationException($"Guest ID {guestId} not found.");

            Room room = FindRoomByNumber(roomNumber)
                ?? throw new InvalidOperationException($"Room number {roomNumber} not found.");

            foreach (Reservation existing in _reservations)
            {
                if (existing.Room.RoomNumber != roomNumber)
                    continue;

                bool isActive = existing.Status != Reservation.ReservationSatus.Canceled
                             && existing.Status != Reservation.ReservationSatus.CheckOut;

                if (!isActive)
                    continue;

                bool isOverlapping = checkIn < existing.CheckOutDate && checkOut > existing.CheckInDate;
                if (isOverlapping)
                {
                    throw new InvalidOperationException(
                        $"Room {roomNumber} is already booked from {existing.CheckInDate:yyyy-MM-dd} to {existing.CheckOutDate:yyyy-MM-dd}.");
                }
            }

            Reservation reservation = guest.MakeReservationInTheHotel(reservationId, checkIn, checkOut, room);
            _reservations.Add(reservation);
            return reservation;
        }

        public void ConfirmReservation(int reservationId) => GetReservationOrThrow(reservationId).Confirm();

        public void CheckInReservation(int reservationId) => GetReservationOrThrow(reservationId).CheckIn();

        public void CheckOutReservation(int reservationId) => GetReservationOrThrow(reservationId).CheckOut();

        public void CancelReservation(int reservationId) => GetReservationOrThrow(reservationId).Cancel();

        public void StartRoomMaintenance(int roomNumber) => GetRoomOrThrow(roomNumber).StartMaintaince();

        public void EndRoomMaintenance(int roomNumber) => GetRoomOrThrow(roomNumber).EndMaintaince();

        public void UpdateRoomRate(int roomNumber, decimal newRate) => GetRoomOrThrow(roomNumber).ChangeNightlyRate(newRate);

        private Guest? FindGuestById(int id)
        {
            foreach (Guest g in _guests)
                if (g.GuestId == id) return g;
            return null;
        }

        private Room? FindRoomByNumber(int roomNumber)
        {
            foreach (Room r in _rooms)
                if (r.RoomNumber == roomNumber) return r;
            return null;
        }

        private Reservation? FindReservationById(int id)
        {
            foreach (Reservation r in _reservations)
                if (r.ReservationId == id) return r;
            return null;
        }

        private Reservation GetReservationOrThrow(int id) =>
            FindReservationById(id) ?? throw new InvalidOperationException($"Reservation ID {id} not found.");

        private Room GetRoomOrThrow(int roomNumber) =>
            FindRoomByNumber(roomNumber) ?? throw new InvalidOperationException($"Room number {roomNumber} not found.");
    }
}