namespace RandevuYonetimSistemi.Models.Entities
{
    public class TrainerAvailability
    {
        public int Id { get; set; }

        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = default!;

        public int DayOfWeek { get; set; } // 0=Pazar ... 6=Cumartesi
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

    }
}