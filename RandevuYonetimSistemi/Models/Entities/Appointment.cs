using System.ComponentModel.DataAnnotations;

namespace RandevuYonetimSistemi.Models.Entities
{
    public enum AppointmentStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        Cancelled = 3
    }

    public class Appointment
    {
        public int Id { get; set; }

        // Tek salon mantığı
        public int GymId { get; set; }
        public Gym Gym { get; set; } = default!;

        // Üye (Identity User)
        [Required]
        public string MemberId { get; set; } = default!;
        public RandevuYonetimSistemi.Models.ApplicationUser Member { get; set; } = default!;

        // Trainer/Service
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = default!;

        public int ServiceId { get; set; }
        public Service Service { get; set; } = default!;

        // Zaman
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }

        // Snapshot (randevu anındaki detaylar saklansın)
        [Required, StringLength(80)]
        public string TrainerNameSnapshot { get; set; } = default!;

        [Required, StringLength(80)]
        public string ServiceNameSnapshot { get; set; } = default!;

        public int DurationMinutesSnapshot { get; set; }
        public decimal PriceSnapshot { get; set; }

        public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

        [StringLength(300)]
        public string? DecisionNote { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
