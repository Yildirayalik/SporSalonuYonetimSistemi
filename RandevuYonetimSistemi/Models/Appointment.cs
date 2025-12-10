namespace RandevuYonetimSistemi.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = null!;

        public int GymServiceId { get; set; }
        public GymService GymService { get; set; } = null!;

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public decimal Price { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

    }
}