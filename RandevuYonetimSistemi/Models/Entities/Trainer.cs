using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace RandevuYonetimSistemi.Models.Entities
{
    public class Trainer
    {
            public int Id { get; set; }

            public int GymId { get; set; }
            [ValidateNever]
            public Gym Gym { get; set; } = default!;

            [Required, StringLength(80)]
            public string FullName { get; set; } = default!;

            [StringLength(120)]
            public string? Email { get; set; }

            [StringLength(20)]
            public string? Phone { get; set; }

            [StringLength(500)]
            public string? Bio { get; set; }

            public bool IsActive { get; set; } = true;

            // ilişkiler
            public ICollection<TrainerServiceMap> TrainerServiceMaps { get; set; } = new List<TrainerServiceMap>();
            public ICollection<TrainerAvailability> TrainerAvailabilities { get; set; } = new List<TrainerAvailability>();
            public ICollection<TrainerSpecialtyMap> TrainerSpecialtyMaps { get; set; } = new List<TrainerSpecialtyMap>();
            public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
            public ICollection<TrainerAvailability> Availabilities { get; set; } = new List<TrainerAvailability>();

    }


}