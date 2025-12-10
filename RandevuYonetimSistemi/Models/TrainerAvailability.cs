namespace RandevuYonetimSistemi.Models
{
    public class TrainerAvailability
    {
        public int Id { get; set; }

        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = null!;

        public DayOfWeek DayOfWeek { get; set; } // Pazartesi, Salı vb.
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

    }
}