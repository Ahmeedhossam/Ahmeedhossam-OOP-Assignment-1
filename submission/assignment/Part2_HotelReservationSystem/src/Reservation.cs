namespace Part2_HotelReservationSystem
{
    internal sealed class Reservation
    {
        private Reservation(int id, DateTimeOffset checkInDate, DateTimeOffset checkOutDate, Room room)
        {
            if (id <= 0 || checkInDate >= checkOutDate)
            {
                throw new ArgumentException();
            }
            if (room is null)
            {
                throw new ArgumentNullException(nameof(room));
            }
            if (room.RoomIsUnderMaintaince is true)
            {
                throw new InvalidOperationException("room is under maintaince so can not reserve");
            }

            ReservationId = id;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            this.Room = room;

            this.Status = ReservationSatus.Pending;
        }

        public int ReservationId { get; }
        public DateTimeOffset CheckInDate { get; }
        public DateTimeOffset CheckOutDate { get; }
        public ReservationSatus Status { get; private set; }
        public Room Room { get; }
        public decimal ReservationTotalCost => this.Room.NightlyRate * (decimal)(CheckOutDate - CheckInDate).TotalDays;

        public enum ReservationSatus
        {
            Pending,
            Confirmed,
            CheckIn,
            CheckOut,
            Canceled
        }

        public static Reservation CreateReservationDetails(int Reservationid, DateTimeOffset Checkin, DateTimeOffset Checkout, Room room)
        {
            return new Reservation(Reservationid, Checkin, Checkout, room);
        }

        public void Confirm()
        {
            if (Status != ReservationSatus.Pending)
            {
                throw new InvalidOperationException("Only pending reservations can be confirmed.");
            }

            Status = ReservationSatus.Confirmed;
        }

        public void CheckIn()
        {
            if (Status != ReservationSatus.Confirmed)
            {
                throw new InvalidOperationException("Cannot check in unless the reservation is confirmed.");
            }

            Status = ReservationSatus.CheckIn;
        }

        public void CheckOut()
        {
            if (Status != ReservationSatus.CheckIn)
            {
                throw new InvalidOperationException("Cannot check out unless the guest is currently checked in.");
            }

            Status = ReservationSatus.CheckOut;
        }

        public void Cancel()
        {
            if (Status != ReservationSatus.Pending && Status != ReservationSatus.Confirmed)
            {
                throw new InvalidOperationException("Only pending or confirmed reservations can be cancelled.");
            }

            Status = ReservationSatus.Canceled;
        }
    }
}