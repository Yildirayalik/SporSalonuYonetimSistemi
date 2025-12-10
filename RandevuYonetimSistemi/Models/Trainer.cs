namespace RandevuYonetimSistemi.Models
{
    public class Trainer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Specialization { get; set; } = null!; // Kas geliştirme, kilo verme, yoga vb.
        public string Bio { get; set; } = null!;

        public int FitnessCenterId { get; set; }
        public FitnessCenter FitnessCenter { get; set; } = null!;

        public ICollection<TrainerService> TrainerServices { get; set; } = new List<TrainerService>();
        public ICollection<TrainerAvailability> Availabilities { get; set; } = new List<TrainerAvailability>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    }
}