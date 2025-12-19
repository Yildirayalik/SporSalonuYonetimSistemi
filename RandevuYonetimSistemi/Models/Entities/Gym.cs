using System.ComponentModel.DataAnnotations;
namespace RandevuYonetimSistemi.Models.Entities
{
    public class Gym
    {

        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public TimeOnly OpenTime { get; set; }
        public TimeOnly CloseTime { get; set; }
        public string Address { get; set; } = default!;
        public string Phone { get; set; } = default!;

        public ICollection<Service> Services { get; set; } = new List<Service>();
        public ICollection<Trainer> Trainers { get; set; } = new List<Trainer>();

    }
}
