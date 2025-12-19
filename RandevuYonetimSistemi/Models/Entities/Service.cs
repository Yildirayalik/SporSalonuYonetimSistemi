using System.ComponentModel.DataAnnotations;

namespace RandevuYonetimSistemi.Models.Entities
{
    public class Service
    {
        public int Id { get; set; }

        public int GymId { get; set; }

        // ✅ nullable yap -> artık formdan Gym beklemez
        public Gym? Gym { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; } = default!;

        [Range(10, 300)]
        public int DurationMinutes { get; set; }

        [Range(0, 100000)]
        public decimal Price { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<TrainerServiceMap> TrainerServiceMaps { get; set; } = new List<TrainerServiceMap>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
