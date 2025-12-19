namespace RandevuYonetimSistemi.Models.Entities
{
    public class Specialty
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        public ICollection<TrainerSpecialtyMap> TrainerSpecialtyMaps { get; set; } = new List<TrainerSpecialtyMap>();

    }
}
