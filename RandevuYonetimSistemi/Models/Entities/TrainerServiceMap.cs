namespace RandevuYonetimSistemi.Models.Entities
{
    public class TrainerServiceMap
    {
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = default!;

        public int ServiceId { get; set; }
        public Service Service { get; set; } = default!;
    }
}