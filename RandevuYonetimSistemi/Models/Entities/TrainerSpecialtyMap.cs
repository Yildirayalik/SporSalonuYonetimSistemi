namespace RandevuYonetimSistemi.Models.Entities
{
    public class TrainerSpecialtyMap
    {
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = default!;
        public int SpecialtyId { get; set; }
        public Specialty Specialty { get; set; } = default!;
    }
}