namespace Part2_HotelReservationSystem
{
    internal sealed class Room
    {
        private Room(int roomNumber, decimal nightlyRate, bool roomIsUnderMaintaince, RoomType Type)
        {
            if (roomNumber <= 0 || nightlyRate <= 0)
            {
                throw new ArgumentException();
            }
            RoomNumber = roomNumber;
            NightlyRate = nightlyRate;
            RoomIsUnderMaintaince = roomIsUnderMaintaince;
            this.Room_Type = Type;
        }

        public int RoomNumber { get; }
        public decimal NightlyRate { get; private set; }
        public bool RoomIsUnderMaintaince { get; private set; } = false;
        public RoomType Room_Type { get; }

        public enum RoomType
        {
            Single,
            Double,
            Suite
        }

        public void StartMaintaince()
        {
            this.RoomIsUnderMaintaince = true;
        }

        public void EndMaintaince()
        {
            this.RoomIsUnderMaintaince = false;
        }

        public void ChangeNightlyRate(decimal nightlyRate)
        {
            if (nightlyRate <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            this.NightlyRate = nightlyRate;
        }

        public static Room CreateRoom(int RoomNum, decimal Nightrate, bool UnderMaintaince, RoomType Room)
        {
            return new Room(RoomNum, Nightrate, UnderMaintaince, Room);
        }
    }
}